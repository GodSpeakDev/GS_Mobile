﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using Android.Content.PM;
using Xamarin.Forms;

using Java.Util;

using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;

using MvvmCross.Plugins.Messenger;

namespace GodSpeak.Droid
{
	public class ReminderService : IReminderService
	{
		private ISettingsService _settingsService;

		private AlarmManager _alarmManager;
		public AlarmManager AlarmManager
		{
			get
			{
				if (_alarmManager != null)
				{
					return _alarmManager;
				}

				var activity = (Forms.Context as Activity);

				return _alarmManager ?? (_alarmManager = (AlarmManager)activity.GetSystemService(Android.Content.Context.AlarmService));
			}
		}

		public ReminderService(ISettingsService settingsService)
		{
			_settingsService = settingsService;
		}

		public bool SetMessageReminder(Message message)
		{
			//var context = Android.App.Application.Context;

			//// Instantiate the builder and set notification elements:

			var intent = CreateAlarmIntent(message);
			var pendingIntent = CreatePendingIntent(intent, PendingIntentFlags.CancelCurrent);

			var calendar = Calendar.Instance;
			calendar.TimeInMillis = Java.Lang.JavaSystem.CurrentTimeMillis();
			calendar.Set(
				message.DateTimeToDisplay.Year,
				message.DateTimeToDisplay.Month-1,
				message.DateTimeToDisplay.Day,
				message.DateTimeToDisplay.Hour,
				message.DateTimeToDisplay.Minute);

			AlarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);

			return true;
		}

		private Intent CreateAlarmIntent(Message message)
		{
			Intent intent = new Intent(Xamarin.Forms.Forms.Context, typeof(ReminderReceiver));

			var contents = JsonConvert.SerializeObject(message);
			intent.PutExtra("item_json", contents);
			return intent;
		}

		private PendingIntent CreatePendingIntent(Intent intent, PendingIntentFlags flag)
		{
			var random = new System.Random(DateTime.Now.Millisecond);
			var id = random.Next();

			var ids = _settingsService.ReminderIds;
			ids.Add(id);
			_settingsService.ReminderIds = ids;

			return PendingIntent.GetBroadcast(Xamarin.Forms.Forms.Context, id, intent, flag);
		}

		public void ClearReminders()
		{
			foreach (var id in _settingsService.ReminderIds)
			{
				var intent = new Intent(Xamarin.Forms.Forms.Context, typeof(ReminderReceiver));
				var pendingIntent = PendingIntent.GetBroadcast(Xamarin.Forms.Forms.Context, id, intent, PendingIntentFlags.CancelCurrent);
				pendingIntent.Cancel();
			}
		}
	}

	[BroadcastReceiver]
	public class ReminderReceiver : BroadcastReceiver
	{
		private NotificationManager _notificationManager;
		public NotificationManager NotificationManager
		{
			get
			{
				if (_notificationManager != null)
				{
					return _notificationManager;
				}

				var activity = (Forms.Context as Activity);

				return _notificationManager ?? (_notificationManager = (NotificationManager)activity.GetSystemService(Android.Content.Context.NotificationService));
			}
		}

		public override void OnReceive(Context context, Intent intent)
		{
			var message = JsonConvert.DeserializeObject<Message>(intent.GetStringExtra("item_json"));

			if (MainActivity.IsForeground)
			{
				ShowMessage(message);
			}
			else
			{
				SendNotification(message, context);
			}

			var hasMessenger = Mvx.CanResolve<IMvxMessenger>();
			if (hasMessenger)
			{
				Mvx.Resolve<IMvxMessenger>().Publish(new MessageDeliveredMessage(this));	
			}
		}

		private void ShowMessage(Message message)
		{
			var alertDialog = new AlertDialog.Builder(Xamarin.Forms.Forms.Context)
											 .SetTitle("God Speak")
											 .SetMessage(message.Verse.Text)
											 .SetCancelable(false)
											 .SetPositiveButton("Ok", (sender, e) => { });
			alertDialog.Show();
		}

		private void SendNotification(Message message, Context context)
		{			
			Notification.Builder builder = new Notification.Builder(context)
				.SetContentTitle("God Speak")
				.SetSmallIcon(Resource.Drawable.app_icon)
				.SetContentText(message.Verse.Text);

			var random = new System.Random(DateTime.Now.Millisecond);
			var id = random.Next();

			NotificationManager.Notify(message.Id.ToString(), id, builder.Build());
		}
	}
}

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

namespace GodSpeak.Droid
{
	public class ReminderService : IReminderService
	{
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

		public bool SetMessageReminder(Message message)
		{
			//var context = Android.App.Application.Context;

			//// Instantiate the builder and set notification elements:

			var intent = CreateAlarmIntent(message);
			var pendingIntent = CreatePendingIntent(intent, PendingIntentFlags.CancelCurrent);

			AlarmManager.Set(AlarmType.RtcWakeup, (long)message.DateTimeToDisplay.ToUniversalTime().Subtract(
					new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				).TotalMilliseconds, pendingIntent);

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
			var random = new Random(DateTime.Now.Millisecond);
			var id = random.Next();

			return PendingIntent.GetBroadcast(Xamarin.Forms.Forms.Context, id, intent, flag);
		}

		public void ClearReminders()
		{
			
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
		}

		private void ShowMessage(Message message)
		{
			var alertDialog = new AlertDialog.Builder(Xamarin.Forms.Forms.Context)
											 .SetTitle("God Speak")
											 .SetMessage(message.Text)
											 .SetCancelable(false)
											 .SetPositiveButton("Ok", (sender, e) => { });
			alertDialog.Show();
		}

		private void SendNotification(Message message, Context context)
		{			
			Notification.Builder builder = new Notification.Builder(context)
				.SetContentTitle("God Speak")
				.SetSmallIcon(Resource.Drawable.icon)
				.SetContentText(message.Text);

			var random = new Random(DateTime.Now.Millisecond);
			var id = random.Next();

			NotificationManager.Notify(message.MessageId.ToString(), id, builder.Build());
		}
	}
}
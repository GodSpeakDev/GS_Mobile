using System;
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

using Android.Support.V4;

namespace GodSpeak.Droid
{
	public class ReminderService : IReminderService
	{
		private ISettingsService _settingsService;
        private ILoggingService _logger;

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

		public ReminderService(ISettingsService settingsService, ILogManager logManager)
		{
			_settingsService = settingsService;
            _logger = logManager.GetLog();
		}

		public void AddReminderNotification()
		{
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
				message.DateTimeToDisplay.Month - 1,
				message.DateTimeToDisplay.Day,
				//DateTime.Now.Hour,
				//DateTime.Now.Minute + 1);
				message.DateTimeToDisplay.Hour,
				message.DateTimeToDisplay.Minute);

			AlarmManager.Set(AlarmType.RtcWakeup, calendar.TimeInMillis, pendingIntent);

			_logger.Trace(string.Format("ADDED REMINDER: Id: {0} DateToDisplay: {1} FireDate: {2} Message: {3}", message.Id, message.DateTimeToDisplay, calendar, message.Verse.Text));
			System.Diagnostics.Debug.WriteLine(string.Format("ADDED REMINDER: Id: {0} DateToDisplay: {1} FireDate: {2} Message: {3}", message.Id, message.DateTimeToDisplay, calendar, message.Verse.Text));

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

	[BroadcastReceiver(Exported=true)]
	public class ReminderReceiver : Android.Support.V4.Content.WakefulBroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if (MainActivity.IsForeground)
			{
				var message = JsonConvert.DeserializeObject<Message>(intent.GetStringExtra("item_json"));
				ShowMessage(message);
				var hasMessenger = Mvx.CanResolve<IMvxMessenger>();
				if (hasMessenger)
				{
					Mvx.Resolve<IMvxMessenger>().Publish(new MessageDeliveredMessage(this));

					var azureApi = Mvx.Resolve<IWebApiService>();

					if (azureApi != null)
					{
						var messageTitle = message.Verse.Title;
						azureApi.RecordMessageDelivered(new RecordMessageDeliveredRequest()
						{
							VerseCode = messageTitle.ToString(),
							DateDelivered = DateTime.Now
						});
					}
				}
			}
			else
			{
				var newIntent = new Intent(context, typeof(AlarmServiceIntentService));
				newIntent.ReplaceExtras(intent);                           
				StartWakefulService(context, newIntent);
			}
		}

		private void ShowMessage(Message message)
		{
			var alertDialog = new AlertDialog.Builder(Xamarin.Forms.Forms.Context)
								 .SetTitle("GodSpeak")
								 .SetMessage(new VerseFormatter().Convert(message.Verse.Text, null, null, null).ToString())
								 .SetCancelable(false)
								 .SetPositiveButton("Ok", (sender, e) => { });
			alertDialog.Show();
		}
	}

	[Service]
	public class AlarmServiceIntentService : IntentService
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

				return _notificationManager ?? (_notificationManager = (NotificationManager)ApplicationContext.GetSystemService(Android.Content.Context.NotificationService));
			}
		}

		public AlarmServiceIntentService() : base("AlarmServiceIntentService")
		{
			
		}

		protected override void OnHandleIntent(Intent intent)
		{
			try
			{
				var message = JsonConvert.DeserializeObject<Message>(intent.GetStringExtra("item_json"));
				SendNotification(message);
			}
			catch (Exception ex)
			{

			}
			finally
			{
				Android.Support.V4.Content.WakefulBroadcastReceiver.CompleteWakefulIntent(intent);
			}
		}

		private void SendNotification(Message message)
		{
			Notification.Builder builder = new Notification.Builder(ApplicationContext)
				.SetContentTitle("GodSpeak")
				.SetSmallIcon(Resource.Drawable.app_icon)
				.SetContentText(new VerseFormatter().Convert(message.Verse.Text, null, null, null).ToString());

			var random = new System.Random(DateTime.Now.Millisecond);
			var id = random.Next();

			NotificationManager.Notify(message.Id.ToString(), id, builder.Build());
		}
	}
}

using System;
using UIKit;
using Foundation;
using System.Diagnostics;
using GodSpeak.Resources;

namespace GodSpeak.iOS
{
    public class ReminderService : IReminderService
    {
		public static int LocalNotificationLimit = 63;
        public static string MessageIdKey = "messageId";
		public static string MessageTitleKey = "messageTitle";

        private ILoggingService _logger;

        public ReminderService(ILogManager logManager)
        {
            _logger = logManager.GetLog();
        }

        public bool SetMessageReminder (Message message)
        {
            if (IsReminderSet (message) || UIApplication.SharedApplication.ScheduledLocalNotifications.Length == LocalNotificationLimit) {
                return false;
            }

            AddLocalNotification (message);

            return true;
        }

        public void ClearReminders ()
        {
            foreach (UILocalNotification notification in UIApplication.SharedApplication.ScheduledLocalNotifications) {
                UIApplication.SharedApplication.CancelLocalNotification (notification);
            }
        }

        private void AddLocalNotification (Message message)
        {
            var keys = new object [] { MessageIdKey, MessageTitleKey };
            var objects = new object [] { message.Id.ToString (), message.Verse.Title };

            var date = new DateTime (message.DateTimeToDisplay.Year, message.DateTimeToDisplay.Month, message.DateTimeToDisplay.Day);            

            date = date.AddHours (message.DateTimeToDisplay.Hour);
            date = date.AddMinutes (message.DateTimeToDisplay.Minute);

            UILocalNotification notification = new UILocalNotification {				
                FireDate = date.ToNSDate (),
                TimeZone = NSTimeZone.LocalTimeZone,
                AlertBody = new VerseFormatter ().Convert (string.Format ("{0}\n-{1}", message.Verse.Text, message.Verse.Title), null, null, null).ToString (),
                RepeatInterval = 0,
                UserInfo = NSDictionary.FromObjectsAndKeys (objects, keys),
                SoundName = UILocalNotification.DefaultSoundName,
                ApplicationIconBadgeNumber = 1
            };

			_logger.Trace(string.Format("ADDED REMINDER: Id: {0} DateToDisplay: {1} FireDate: {2} Message: {3}", message.Id, message.DateTimeToDisplay, notification.FireDate, message.Verse.Text)); 
            Debug.WriteLine (string.Format("ADDED REMINDER: Id: {0} DateToDisplay: {1} FireDate: {2} Message: {3}", message.Id, message.DateTimeToDisplay, notification.FireDate, message.Verse.Text));
            UIApplication.SharedApplication.ScheduleLocalNotification (notification);
        }

		public void AddReminderNotification()
		{
			UILocalNotification previousNotificationReminder = null;
			NSDate lastDate = NSDate.Now;
			foreach (UILocalNotification scheduleNotification in UIApplication.SharedApplication.ScheduledLocalNotifications)
			{
				if (scheduleNotification.AlertBody == Text.OpenGodSpeakReminder)
				{
					previousNotificationReminder = scheduleNotification;
				}
				else
				{
					lastDate = lastDate.LaterDate(scheduleNotification.FireDate);
				}
			}

			if (previousNotificationReminder != null)
			{
				UIApplication.SharedApplication.CancelLocalNotification(previousNotificationReminder);
			}

			lastDate = lastDate.AddSeconds(60 * 60);

			UILocalNotification notification = new UILocalNotification
			{
				FireDate = lastDate,
				TimeZone = NSTimeZone.LocalTimeZone,
				AlertBody = Text.OpenGodSpeakReminder,
				RepeatInterval = 0,	
				SoundName = UILocalNotification.DefaultSoundName,
				ApplicationIconBadgeNumber=0     
			};

			_logger.Trace(string.Format("ADDED OPEN GODSPEAK MESSAGE: FireDate: {0}", notification.FireDate)); 
            Debug.WriteLine (string.Format("ADDED OPEN GODSPEAK MESSAGE: FireDate: {0}", notification.FireDate)); 
            UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}

        private bool IsReminderSet (Message message)
        {
            foreach (UILocalNotification notification in UIApplication.SharedApplication.ScheduledLocalNotifications) 
			{
                var messageId = notification.UserInfo.ValueForKey (new Foundation.NSString (MessageIdKey));

                if (messageId != null && messageId.ToString () == message.Id.ToString ()) 
				{
                    return true;
                }
            }

            return false;
        }
    }
}

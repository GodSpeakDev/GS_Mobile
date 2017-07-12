using System;
using UIKit;
using Foundation;
using System.Diagnostics;

namespace GodSpeak.iOS
{
    public class ReminderService : IReminderService
    {
        public static string MessageIdKey = "messageId";
		public static string MessageTitleKey = "messageTitle";

        private ILoggingService _logger;

        public ReminderService(ILogManager logManager)
        {
            _logger = logManager.GetLog();
        }

        public bool SetMessageReminder (Message message)
        {
            if (IsReminderSet (message) || UIApplication.SharedApplication.ScheduledLocalNotifications.Length == 64) {
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

        private bool IsReminderSet (Message message)
        {
            foreach (UILocalNotification notification in UIApplication.SharedApplication.ScheduledLocalNotifications) {
                var messageId = notification.UserInfo.ValueForKey (new Foundation.NSString (MessageIdKey));

                if (messageId.ToString () == message.Id.ToString ()) {
                    return true;
                }
            }

            return false;
        }
    }
}

using System;
using UIKit;
using Foundation;
using System.Diagnostics;

namespace GodSpeak.iOS
{
    public class ReminderService : IReminderService
    {
        private static string MessageIdKey = "messageId";

        public bool SetMessageReminder (Message message)
        {
            if (IsReminderSet (message)) {
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
            var keys = new object [] { MessageIdKey };
            var objects = new object [] { message.Id.ToString () };

			var date = new DateTime(message.DateTimeToDisplay.Year, message.DateTimeToDisplay.Month, message.DateTimeToDisplay.Day);
			//date = date.AddHours(DateTime.Now.Hour);
			//date = date.AddMinutes(DateTime.Now.Minute + 5);

			date = date.AddHours(message.DateTimeToDisplay.Hour);
			date = date.AddMinutes(message.DateTimeToDisplay.Minute);

			UILocalNotification notification = new UILocalNotification
			{
				FireDate = date.ToNSDate(),
				TimeZone = NSTimeZone.LocalTimeZone,
				AlertBody = new VerseFormatter().Convert(message.Verse.Text, null, null, null).ToString(),
                RepeatInterval = 0,
                UserInfo = NSDictionary.FromObjectsAndKeys (objects, keys),
                SoundName = UILocalNotification.DefaultSoundName,
                ApplicationIconBadgeNumber = 1

            };
            Debug.WriteLine ("Setting reminder: " + notification.FireDate);
            UIApplication.SharedApplication.ScheduleLocalNotification (notification);
        }

        private bool IsReminderSet (Message message)
        {
            foreach (UILocalNotification notification in UIApplication.SharedApplication.ScheduledLocalNotifications) {
                var messageId = notification.UserInfo.ValueForKey (new Foundation.NSString ("messageId"));

                if (messageId.ToString () == message.Id.ToString ()) {
                    return true;
                }
            }

            return false;
        }
    }
}

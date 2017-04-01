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
            var objects = new object [] { message.MessageId.ToString () };

            UILocalNotification notification = new UILocalNotification {
                FireDate = message.DateTimeToDisplay.ToNSDate (),
                TimeZone = NSTimeZone.LocalTimeZone,
                AlertBody = message.Verse.Text,
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

                if (messageId.ToString () == message.MessageId.ToString ()) {
                    return true;
                }
            }

            return false;
        }
    }
}

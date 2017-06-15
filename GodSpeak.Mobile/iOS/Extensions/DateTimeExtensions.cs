using System;
using Foundation;

namespace GodSpeak.iOS
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime (this NSDate date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime (
                                     new DateTime (2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds (date.SecondsSinceReferenceDate);
        }

        public static NSDate ToNSDate (this DateTime date)
        {
			var newDate = new DateTime(date.Year, date.Month, date.Day);

			newDate = newDate.AddHours (date.Hour);
            newDate = newDate.AddMinutes (date.Minute);

            if (date.Kind == DateTimeKind.Unspecified) {
                date = DateTime.SpecifyKind (date, DateTimeKind.Local);
            }
            return (NSDate)date;

            //DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime (
            //                         new DateTime (2001, 1, 1, 0, 0, 0));
            //return NSDate.FromTimeIntervalSinceReferenceDate (
            //    (date - reference).TotalSeconds);
        }
    }
}

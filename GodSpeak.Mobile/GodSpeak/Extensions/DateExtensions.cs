using System;

namespace GodSpeak
{
	public static class DateExtensions
	{
		public static TimeSpan ToTimeSpan(this DateTime date)
		{
			var timeSpan = new TimeSpan();
			timeSpan = timeSpan.Add(TimeSpan.FromHours(date.Hour));
			timeSpan = timeSpan.Add(TimeSpan.FromMinutes(date.Minute));
			return timeSpan;
		}

		public static DateTime ToDateTime(this TimeSpan timeSpan)
		{
			var date = new DateTime();
			date.AddHours(timeSpan.Hours);
			date.AddMinutes(timeSpan.Minutes);
			return date;
		}
	}
}

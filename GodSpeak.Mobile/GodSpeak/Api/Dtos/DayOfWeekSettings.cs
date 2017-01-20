using System;
namespace GodSpeak
{
	public class DayOfWeekSettings
	{
		public Guid DayOfWeekSettingsId
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public int NumberOfMessages
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public DateTime StartDateTime
		{
			get;
			set;
		}

		public DateTime EndDateTime
		{
			get;
			set;
		}

		public int Weekday
		{
			get;
			set;
		}
	}
}

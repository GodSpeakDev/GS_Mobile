using System;
using MvvmCross.Core.ViewModels;
               
namespace GodSpeak
{
	public class DayOfWeekSettings : MvxViewModel
	{
		public Guid DayOfWeekSettingsId
		{
			get;
			set;
		}

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled;}
			set { SetProperty(ref _enabled, value);}
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

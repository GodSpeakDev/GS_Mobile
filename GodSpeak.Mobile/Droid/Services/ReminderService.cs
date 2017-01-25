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
			return false;
		}

		public void ClearReminders()
		{
			
		}
	}
}

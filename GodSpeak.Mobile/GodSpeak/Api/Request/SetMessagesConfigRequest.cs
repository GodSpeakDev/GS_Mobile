using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class SetMessagesConfigRequest
	{
		public SetMessagesConfigRequest()
		{
			Settings = new List<DayOfWeekSettings>();
		}

		public string Token
		{
			get;
			set;
		}

		public List<DayOfWeekSettings> Settings
		{
			get;
			set;
		}
	}
}

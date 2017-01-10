using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class MessageConfig
	{
		public MessageConfig()
		{
			
		}

		public int MessagesPerDay
		{
			get;
			set;
		}

		public List<DailyConfig> Days
		{
			get;
			set;
		}
	}
}

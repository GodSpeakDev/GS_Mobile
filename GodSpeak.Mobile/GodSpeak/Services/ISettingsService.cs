﻿using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public interface ISettingsService
	{
		List<string> DeliveredVerseCodes
		{
			get;
			set;
		}

		List<int> ReminderIds
		{
			get;
			set;
		}

		string Token
		{
			get;
			set;
		}

		string Email
		{
			get;
			set;
		}

		float Latitude
		{
			get;
			set;
		}

		float Longitude
		{
			get;
			set;
		}
	}
}

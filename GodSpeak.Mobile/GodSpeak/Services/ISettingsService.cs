using System;
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
	}
}

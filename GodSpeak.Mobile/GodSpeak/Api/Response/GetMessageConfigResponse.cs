using System;
using System.Linq;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetMessageConfigResponse
	{
		public GetMessageConfigResponse()
		{
			Payload = new List<DayOfWeekSettings>();
		}

		public List<DayOfWeekSettings> Payload
		{
			get;
			set;
		}
	}
}

using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetImpactResponse
	{
		public GetImpactResponse()
		{
			DeliveredScriptures = new List<DeliveredScripture>();
			GiftedApps = new List<GiftedApp>();
		}

		public List<DeliveredScripture> DeliveredScriptures
		{
			get;
			set;
		}

		public List<GiftedApp> GiftedApps
		{
			get;
			set;
		}
	}
}

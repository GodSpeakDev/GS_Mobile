using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetImpactResponse
	{
		public List<ImpactDay> Payload
		{
			get;
			set;
		}

		public GetImpactResponse()
		{
			Payload = new List<ImpactDay>();	
		}
	}
}

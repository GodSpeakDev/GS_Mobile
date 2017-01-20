using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetCategoriesResponse
	{
		public GetCategoriesResponse()
		{
			Payload = new List<MessageCategory>();
		}

		public List<MessageCategory> Payload
		{
			get;
			set;
		}
	}
}

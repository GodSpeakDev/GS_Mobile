using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetCategoriesResponse
	{
		public GetCategoriesResponse()
		{
			Categories = new List<MessageCategory>();
		}

		public List<MessageCategory> Categories
		{
			get;
			set;
		}
	}
}

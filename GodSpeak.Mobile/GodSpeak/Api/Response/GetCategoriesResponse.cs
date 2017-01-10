using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetCategoriesResponse
	{
		public GetCategoriesResponse()
		{
			Categories = new List<Category>();
		}

		public List<Category> Categories
		{
			get;
			set;
		}
	}
}

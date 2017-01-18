using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class SaveCategoriesRequest
	{
		public SaveCategoriesRequest()
		{
			
		}

		public string Token
		{
			get;
			set;
		}

		public List<int> SelectedIds
		{
			get;
			set;
		}
	}
}

using System;
using System.Linq;
using System.Collections.Generic;

namespace GodSpeak
{
	public class SaveCategoriesResponse
	{
		public SaveCategoriesResponse()
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

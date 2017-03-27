using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class SaveCategoriesRequest
	{
		public SaveCategoriesRequest()
		{
			Payload = new List<MessageCategory>();
		}

		public string Token
		{
			get;
			set;
		}

		public List<MessageCategory> Payload
		{
			get;
			set;
		}
	}
}

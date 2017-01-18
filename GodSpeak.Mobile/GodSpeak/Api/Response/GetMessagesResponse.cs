using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetMessagesResponse
	{
		public GetMessagesResponse()
		{
			Messages = new List<Message>();
		}

		public List<Message> Messages
		{
			get;
			set;
		}
	}
}

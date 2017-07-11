using System;

namespace GodSpeak
{
	public class ShareRequest
	{
		public string[] ToEmailAddresses
		{
			get;
			set;
		}

		public string FromEmailAddress
		{
			get;
			set;
		}

		public string FromName
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string Subject
		{
			get;
			set;
		}
	}
}
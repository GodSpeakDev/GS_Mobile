using System;
namespace GodSpeak
{
	public class GetMessagesRequest
	{
		public string Token
		{
			get;
			set;
		}

		public int PageNumber
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}
	}
}

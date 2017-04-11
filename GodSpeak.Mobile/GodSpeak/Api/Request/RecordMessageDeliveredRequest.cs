using System;
namespace GodSpeak
{
	public class RecordMessageDeliveredRequest
	{
		public DateTime DateDelivered
		{
			get;
			set;
		}

		public string VerseCode
		{
			get;
			set;
		}

		public string Token
		{
			get;
			set;   
		}
	}
}

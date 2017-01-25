using System;
namespace GodSpeak
{
	public class Message
	{
		public Guid MessageId
		{
			get;
			set;
		}

		public DateTime DateTimeToDisplay
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public string Image
		{
			get;
			set;
		}

		public string SubTitle
		{
			get;
			set;
		}
	}
}

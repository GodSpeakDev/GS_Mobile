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

		public string BeforeVerse
		{
			get;
			set;
		}

		public string AfterVerse
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public string BeforeAuthor
		{
			get;
			set;
		}

		public string Author
		{
			get;
			set;
		}

		public string AfterAuthor
		{
			get;
			set;
		}
	}
}

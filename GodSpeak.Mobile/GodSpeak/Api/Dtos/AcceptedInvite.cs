using System;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class AcceptedInvite
	{		
		public string ImageUrl
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string SubTitle
		{
			get;
			set;
		}

		public string ButtonTitle
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

		public string EmailAddress
		{
			get;
			set;
		}

		public int GiftsGiven
		{
			get;
			set;
		}

		public DateTime DateClaimed
		{
			get;
			set;
		}

		public string DateClaimedDescription
		{
			get { return string.Format(Text.ClaimedDateDescription, DateClaimed); }
		}

	}
}

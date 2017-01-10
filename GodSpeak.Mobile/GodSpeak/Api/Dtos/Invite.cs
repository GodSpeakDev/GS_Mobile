using System;
namespace GodSpeak
{
	public class Invite
	{
		public long Id
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public bool IsAlreadyUsed
		{
			get;
			set;
		}
	}
}

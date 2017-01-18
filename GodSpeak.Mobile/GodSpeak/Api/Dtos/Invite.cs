using System;
namespace GodSpeak
{
	public class Invite
	{
		public Guid InviteId
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

		public string InviterEmail
		{
			get;
			set;
		}

		public string RedeemerEmail
		{
			get;
			set;
		}

		public bool Redeemed
		{
			get;
			set;
		}

		public string ReedemedDescription
		{
			get 
			{
				return string.Format($"{Code} - {RedeemerEmail}");
			}
		} 
	}
}

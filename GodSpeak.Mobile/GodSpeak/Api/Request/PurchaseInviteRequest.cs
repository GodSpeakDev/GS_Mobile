using System;
namespace GodSpeak
{
	public class PurchaseInviteRequest
	{
		public string Token
		{
			get;
			set;
		}

		public Guid InviteBundleId
		{
			get;
			set;
		}
	}
}

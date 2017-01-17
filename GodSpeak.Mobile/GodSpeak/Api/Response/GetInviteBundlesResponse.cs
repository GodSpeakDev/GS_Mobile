using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetInviteBundlesResponse
	{
		public List<InviteBundle> Payload
		{
			get;
			set;
		}

		public GetInviteBundlesResponse()
		{
			Payload = new List<InviteBundle>();
		}
	}
}

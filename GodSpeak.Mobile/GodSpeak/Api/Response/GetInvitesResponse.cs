using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetInvitesResponse
	{
		public GetInvitesResponse()
		{
			Payload = new List<Invite>();
		}

		public List<Invite> Payload
		{
			get;
			set;
		}
	}
}

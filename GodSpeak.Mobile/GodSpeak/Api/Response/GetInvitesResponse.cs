using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class GetInvitesResponse
	{
		public GetInvitesResponse()
		{
			Invites = new List<Invite>();
		}

		public List<Invite> Invites
		{
			get;
			set;
		}
	}
}

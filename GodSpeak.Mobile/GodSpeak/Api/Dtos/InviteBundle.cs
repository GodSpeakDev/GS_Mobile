using System;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class InviteBundle
	{
		public Guid InviteBundleId
		{
			get;
			set;
		}

		public int NumOfInvites
		{
			get;
			set;
		}

		public string ItunesId
		{
			get;
			set;
		}

		public string PlaystoreId
		{
			get;
			set;
		}

		public decimal Cost
		{
			get;
			set;
		}

		public string CostDescription
		{
			get;
			set;
		}

		public string Description
		{
			get 
			{
				return string.Format(Text.InviteBundleDescription, NumOfInvites);	
			}
		}
	}
}

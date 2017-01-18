using System;
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

		public override string ToString()
		{
			return string.Format("{0} {1} for {2:C}", NumOfInvites, NumOfInvites > 1 ? "Credits" : "Credit", Cost);
		}
	}
}

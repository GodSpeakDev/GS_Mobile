using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class ImpactDay
	{
		public Guid ImpactDayId
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public int InvitesClaimed
		{
			get;
			set;
		}

		public int ScripturesDelivered
		{
			get;
			set;
		}

		public List<MapPoint> MapPoints
		{
			get;
			set;
		}

		public ImpactDay()
		{
			MapPoints = new List<MapPoint>();
		}
	}
}

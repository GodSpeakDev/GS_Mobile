using System;
using System.Collections.Generic;

namespace GodSpeak
{
	public class ImpactDay
	{
		public DateTime Date
		{
			get;
			set;
		}

		public int InvitesClaimed
		{
			get 
			{
				return Points.Count;
			}

		}

		public int ScripturesDelivered
		{
			get;
			set;
		}

		public List<MapPoint> Points
		{
			get;
			set;
		}

		public ImpactDay()
		{
			Points = new List<MapPoint>();
		}
	}
}

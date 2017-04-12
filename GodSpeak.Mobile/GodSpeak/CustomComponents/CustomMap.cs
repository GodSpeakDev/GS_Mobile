using System;
using Xamarin.Forms.Maps;

namespace GodSpeak
{
	public class CustomMap : Map
	{
		public Action<MapPoint, Pin> OnAddPin
		{
			get;
			set;
		}

		public Action<MapPoint> OnRemovePin
		{
			get;
			set;
		}

		public CustomMap()
		{
			
		}
	}
}

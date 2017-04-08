using System;
using Xamarin.Forms.Maps;

namespace GodSpeak
{
	public class CustomMap : Map
	{
		public Action<Guid, Pin> OnAddPin
		{
			get;
			set;
		}

		public Action<Guid> OnRemovePin
		{
			get;
			set;
		}

		public CustomMap()
		{
			
		}
	}
}

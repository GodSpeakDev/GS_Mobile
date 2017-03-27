using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class ReturnKeyEffect : RoutingEffect
	{
		public string ReturnText { get; set; }

		public ReturnKeyEffect() : base("GodSpeak.ReturnKeyEffect")
		{
			
		}
	}
}

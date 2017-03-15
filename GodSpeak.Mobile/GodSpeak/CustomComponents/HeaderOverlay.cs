using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class HeaderOverlay : Grid
	{
		public HeaderOverlay()
		{
			HeightRequest = 60;
			Padding = new Thickness(10, 20, 10, 0);
			BackgroundColor = ColorHelper.DarkGrayNavBar;
		}
	}
}

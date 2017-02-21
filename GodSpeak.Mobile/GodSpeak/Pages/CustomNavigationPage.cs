using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomNavigationPage : NavigationPage
	{
		public CustomNavigationPage(Page page) : base(page)
		{
			BarTextColor = ColorHelper.Secondary;
			BarBackgroundColor = ColorHelper.DarkGray;
		}
	}
}

using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomContentPage : ContentPage
	{
		public CustomContentPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
		}
	}
}

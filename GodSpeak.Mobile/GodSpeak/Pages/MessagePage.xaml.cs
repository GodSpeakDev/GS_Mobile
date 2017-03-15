using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class MessagePage : ContentPage
	{
		public MessagePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}

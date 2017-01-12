using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class WelcomePage : CustomContentPage
	{
		public WelcomePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}

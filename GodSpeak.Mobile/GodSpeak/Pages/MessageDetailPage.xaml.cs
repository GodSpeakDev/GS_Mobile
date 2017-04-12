using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GodSpeak
{
	public partial class MessageDetailPage : CustomContentPage
	{
		public MessageDetailPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}
	}
}

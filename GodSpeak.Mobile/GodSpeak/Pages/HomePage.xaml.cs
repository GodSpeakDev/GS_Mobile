using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;

namespace GodSpeak
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (Device.OS == TargetPlatform.iOS)
			{
				var imageContentSize = this.Height - CitationContent.Height - MenuContent.Height;
				imageContentSize = Math.Min(300, imageContentSize);
				ImageContent.HeightRequest = imageContentSize;
			}
		}
	}
}

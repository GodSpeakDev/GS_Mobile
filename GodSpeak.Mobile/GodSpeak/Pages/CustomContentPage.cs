using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomContentPage : ContentPage
	{
		private CustomViewModel ViewModel
		{
			get { return (CustomViewModel)BindingContext; }
		}

		public CustomContentPage()
		{
			//NavigationPage.SetHasNavigationBar(this, false);
			//Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (ViewModel != null)
			{
				var dialogService = ViewModel.DialogService as DialogService;
				if (dialogService != null)
				{
					dialogService.DoShowAlert = DisplayAlert;
					dialogService.DoShowConfirmation = DisplayAlert;
					dialogService.DoShowMenu = DisplayActionSheet;
				}
			}
		}
	}
}

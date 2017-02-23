using System;
using Xamarin.Forms;
using System.Threading.Tasks;

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
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (ViewModel != null)
			{
				var dialogService = ViewModel.DialogService as DialogService;
				if (dialogService != null)
				{
					dialogService.DoShowAlert = ShowAlert;
					dialogService.DoShowConfirmation = DisplayAlert;
					dialogService.DoShowMenu = ShowMenu;
				}
			}
		}

		private async Task ShowAlert(string title, string message, string cancel)
		{			
			await Task.Delay(1);
			var alertView = new AlertView();
			alertView.Title = title;
			alertView.Message = message;

			var layout = this.Content as AbsoluteLayout;

			layout.Children.Add(alertView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			alertView.Show();
		}

		private async Task<string> ShowMenu(string title, string message, string[] buttons)
		{
			await Task.Delay(1);
			var popupMenu = new ActionSheetPopup();
			popupMenu.Title = title;
			popupMenu.Message = message;
			popupMenu.Buttons = buttons;

			var layout = this.Content as AbsoluteLayout;
			layout.Children.Add(popupMenu, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			return await popupMenu.Show();
		}
	}
}

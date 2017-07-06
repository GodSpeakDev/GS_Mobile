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

		public bool PreventKeyboardOverlap
		{
			get;
			set;
		}

		public CustomContentPage()
		{
			PreventKeyboardOverlap = false;
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
					dialogService.DoShowInputPopup = ShowInputPopup;
				}
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (Device.RuntimePlatform == Device.Android && height > 680)
			{
				this.LayoutChildren(0, 0, width + 1, height + 1);
			}
		}

		private async Task ShowAlert(string title, string message, string cancel)
		{			
			await Task.Delay(1);
			var alertView = new AlertView();
			alertView.Title = title;
			alertView.Message = message;
			alertView.ButtonText = cancel;

			var layout = this.Content as AbsoluteLayout;

			layout.Children.Add(alertView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			await alertView.Show();
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

		private async Task<InputResult> ShowInputPopup(string title, string message, InputOptions inputOptions, string[] buttons)
		{
			await Task.Delay(1);
			var popupMenu = new InputPopup();
			popupMenu.Title = title;
			popupMenu.Message = message;
			popupMenu.Buttons = buttons;
			popupMenu.InputOptions = inputOptions;

			var layout = this.Content as AbsoluteLayout;
			layout.Children.Add(popupMenu, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
			return await popupMenu.Show();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			var viewModel = BindingContext as CustomViewModel;
			if (viewModel != null)
			{
				viewModel.OnAppearing();
			}
		}
	}
}

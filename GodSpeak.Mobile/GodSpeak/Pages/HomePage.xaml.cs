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

			//SettingsItem.GestureRecognizers.Add(new TapGestureRecognizer()
			//{
			//	Command = new MvxCommand(() =>
			//	{
			//		AnimateView(SettingsItem, (this.BindingContext as HomeViewModel).MessageSettingsCommand);
			//	})
			//});

			//LogoutItem.GestureRecognizers.Add(new TapGestureRecognizer()
			//{
			//	Command = new MvxCommand(() =>
			//	{
			//		AnimateView(LogoutItem, (this.BindingContext as HomeViewModel).LogoutCommand);
			//	})
			//});

			//ProfileItem.GestureRecognizers.Add(new TapGestureRecognizer()
			//{
			//	Command = new MvxCommand(() =>
			//	{
			//		AnimateView(ProfileItem, (this.BindingContext as HomeViewModel).MyProfileCommand);
			//	})
			//});
		}

		private void AnimateView(View view, ICommand command)
		{
			var reduceOpacityAnimation = new Animation((x) => 
			{
				view.Opacity = 1 - x * .5;
			});

			var increaseOpacityAnimation = new Animation((x) =>
			{
				view.Opacity = 1 - 0.5 + x * .5;
			}, finished: () =>
			{
				command.Execute(null);
			});

			var animation = new Animation();
			animation.Add(0, 0.5, reduceOpacityAnimation);
			animation.Add(0.5, 1, increaseOpacityAnimation);
			animation.Commit(view, "Tap");
		}
	}
}

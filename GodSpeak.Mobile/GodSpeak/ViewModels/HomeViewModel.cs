using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;

namespace GodSpeak
{
    public class HomeViewModel : MvxMasterDetailViewModel<MessageViewModel>
    {		
		private MvxCommand _logoutCommand;
		public ICommand LogoutCommand
		{
			get
			{
				return _logoutCommand ?? (_logoutCommand = new MvxCommand(() =>
				{
					this.ShowViewModel<GetStartedViewModel>(presentationBundle: NavigationBundles.RestoreNavigationBundle);
				}));
			}
		}

		private MvxCommand _messageSettingsCommand;
		public ICommand MessageSettingsCommand
		{
			get
			{
				return _messageSettingsCommand ?? (_messageSettingsCommand = new MvxCommand(() =>
				{
					this.ShowViewModel<MessageSettingsViewModel>(presentationBundle: NavigationBundles.ClearStackBundle);
				}));
			}
		}

		private MvxCommand _myProfileCommand;
		public ICommand MyProfileCommand
		{
			get
			{
				return _myProfileCommand ?? (_myProfileCommand = new MvxCommand(() =>
				{
					this.ShowViewModel<MyProfileViewModel>(presentationBundle: NavigationBundles.ClearStackBundle);
				}));
			}
		}

		public HomeViewModel()
		{			
		}
    }
}

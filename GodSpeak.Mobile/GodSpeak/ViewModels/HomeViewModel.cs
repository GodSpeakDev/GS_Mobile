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
		private MvxCommand _shareCommand;
		public ICommand ShareCommand
		{
			get
			{
				return _shareCommand ?? (_shareCommand = new MvxCommand(() =>
				{
					this.ChangePresentation(new CloseMenuPresentationHint());
					this.ShowViewModel<ShareViewModel>();
				}));
			}
		}

		private MvxCommand _logoutCommand;
		public ICommand LogoutCommand
		{
			get
			{
				return _logoutCommand ?? (_logoutCommand = new MvxCommand(() =>
				{
					this.ChangePresentation(new CloseMenuPresentationHint());
					this.ShowViewModel<GetStartedViewModel>();
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
					this.ChangePresentation(new CloseMenuPresentationHint());
					this.ShowViewModel<MessageSettingsViewModel>();
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
					this.ChangePresentation(new CloseMenuPresentationHint());
					this.ShowViewModel<MyProfileViewModel>();
				}));
			}
		}

		public HomeViewModel()
		{			
		}
    }
}

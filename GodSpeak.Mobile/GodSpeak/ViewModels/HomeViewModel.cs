using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;
using GodSpeak.Resources;

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
		public MvxCommand LogoutCommand
		{
			get
			{
				return _logoutCommand ?? (_logoutCommand = new MvxCommand(() =>
				{
					this.ChangePresentation(new CloseMenuPresentationHint());
					this.ShowViewModel<LoginViewModel>(presentationBundle:
													   new MvxBundle(new Dictionary<string, string>() 
					{
						{"NavigationMode", "RestoreNavigation"}
					}));
				}));
			}
		}

		private MvxCommand _messageSettingsCommand;
		public MvxCommand MessageSettingsCommand
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
		public MvxCommand MyProfileCommand
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

		private ObservableCollection<MenuItem> _menuItems;
		public ObservableCollection<MenuItem> MenuItems
		{
			get { return _menuItems; }
			set { SetProperty(ref _menuItems, value); }
		}

		public HomeViewModel()
		{
			MenuItems = new ObservableCollection<MenuItem>()
			{
				new MenuItem()
				{
					Command = MessageSettingsCommand,
					Title = Text.MenuMessageSettings,
					Image = "SettingsIcon.png"
				},
				new MenuItem()
				{
					Command = MyProfileCommand,
					Title = Text.MyProfileSettings,
					Image = "profileIcon.png"
				},
				new MenuItem()
				{
					Command = LogoutCommand,
					Title = Text.LogoutSettings,
					Image = "logoutIcon.png"
				}
			};
		}
	}
}


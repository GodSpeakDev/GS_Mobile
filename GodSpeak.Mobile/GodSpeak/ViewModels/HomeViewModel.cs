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
		MenuItem _menuItem;
		public MenuItem SelectedMenu
		{
			get { return _menuItem; }
			set
			{
				if (SetProperty(ref _menuItem, value))
					OnSelectedChangedCommand.Execute(value);
			}
		}

		private MenuItem MessageSettingsMenuItem = new MenuItem { Title = "Message Settings", ViewModelType = typeof(InvitesViewModel)};
		private MenuItem SendInviteMenuItem = new MenuItem { Title = "Send Invite", ViewModelType = typeof(InvitesViewModel)};
		private MenuItem PurchaseInviteMenuItem = new MenuItem { Title = "Purchase Invite", ViewModelType = typeof(PurchaseCreditViewModel) };
		private MenuItem LogoutMenuItem = new MenuItem { Title = "Logout", ViewModelType = typeof(WelcomeViewModel)};

		MvxCommand<MenuItem> _onSelectedChangedCommand;
		ICommand OnSelectedChangedCommand
		{
			get
			{
				return _onSelectedChangedCommand ?? (_onSelectedChangedCommand = new MvxCommand<MenuItem>((item) =>
				{
					if (item == null)
						return;

					var vmType = item.ViewModelType;

					if (item == LogoutMenuItem)
					{	
						ShowViewModel(vmType, presentationBundle: NavigationBundles.RestoreNavigationBundle);      
					}
					else
					{						
						ShowViewModel(vmType, presentationBundle: NavigationBundles.ClearStackBundle);
					}
				}));
			}
		}

		IEnumerable<MenuItem> _menu;
		public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }

		public HomeViewModel()
		{
			Menu = new[] {
				MessageSettingsMenuItem,
				SendInviteMenuItem,
				PurchaseInviteMenuItem,
				LogoutMenuItem
			};
		}

		public override void RootContentPageActivated()
		{
			// When user go backs to root page in NavigationPage (using UI back or changing option in Menu)
			// we unset the SelectedItem of our list
			SelectedMenu = null;
		}
    }

	public class MenuItem
	{
		public string Title { get; set; }
		public Type ViewModelType { get; set; }
	}
}

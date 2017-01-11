using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;

namespace GodSpeak.ViewModels
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

					// We demand to clear the Navigation stack as we are changing the section
					var presentationBundle = new MvxBundle(new Dictionary<string, string> { { "NavigationMode", "ClearStack" } });

					// Show the ViewModel in the Detail NavigationPage
					ShowViewModel(vmType, presentationBundle: presentationBundle);
				}));
			}
		}

		IEnumerable<MenuItem> _menu;
		public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }

		public HomeViewModel()
		{
			Menu = new[] {
				new MenuItem { Title = "Message Settings", ViewModelType = typeof(InvitesViewModel) },
				new MenuItem { Title = "Send Invite", ViewModelType = typeof(InvitesViewModel) },
				new MenuItem { Title = "Purchase Invite", ViewModelType = typeof(InvitesViewModel) },
				new MenuItem { Title = "Logout", ViewModelType = typeof(InvitesViewModel) }
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

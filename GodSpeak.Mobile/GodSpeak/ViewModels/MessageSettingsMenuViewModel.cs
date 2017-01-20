using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;

namespace GodSpeak
{
	public class MessageSettingsMenuViewModel : CustomViewModel
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

		private MenuItem CategoriesMenuItem = new MenuItem { Title = "Categories", ViewModelType = typeof(MessageCategoriesViewModel) };
		private MenuItem DaysTimesMenuItem = new MenuItem { Title = "Days & Times", ViewModelType = typeof(MessageDaysTimesViewModel) };

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

					ShowViewModel(vmType);
				}));
			}
		}

		IEnumerable<MenuItem> _menu;
		public IEnumerable<MenuItem> Menu { get { return _menu; } set { SetProperty(ref _menu, value); } }

		public MessageSettingsMenuViewModel(IDialogService dialogService) : base(dialogService)
		{			
			Menu = new[] {
				CategoriesMenuItem,
				DaysTimesMenuItem
			};
		}
	}
}

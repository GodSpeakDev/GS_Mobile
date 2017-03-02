using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;

namespace GodSpeak
{
	public class MessageSettingsViewModel : CustomViewModel
	{
		private ObservableCollection<SettingsGroup> _groups;
		public ObservableCollection<SettingsGroup> Groups
		{
			get { return _groups;}
			set { SetProperty(ref _groups, value);}
		}

		public MessageSettingsViewModel(IDialogService dialogService) : base(dialogService)
		{			
			
		}
	}

	public class SettingsGroup : MvxViewModel
	{
		private string _sectionTitle;
		public string SectionTitle
		{
			get { return _sectionTitle;}
			set 
			{
				SetProperty(ref _sectionTitle, value);
			}
		}

		private ObservableCollection<SettingsItem> _items;
		public ObservableCollection<SettingsItem> Items
		{
			get { return _items; }
			set { SetProperty(ref _items, value);}
		}
	}

	public class SettingsItem : MvxViewModel
	{
		private string _title;
		public string Title
		{
			get { return _title;}
			set { SetProperty(ref _title, value);}
		}

		private bool _isEnabled;
		public bool IsEnabled
		{
			get { return _isEnabled;}
			set { SetProperty(ref _isEnabled, value);}
		}
	}
}

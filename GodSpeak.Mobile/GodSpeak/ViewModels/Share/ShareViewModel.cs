using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class ShareViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private IShareService _shareService;

		private Tab _unclaimedTab;
		private Tab _claimedTab;

		public ShareViewModel(IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base(dialogService)
		{
			_webApi = webApi;
			_shareService = shareService;

			UnclaimedGiftViewModel = new UnclaimedGiftViewModel(new DialogService(), webApi, shareService);
			ClaimedGiftViewModel = new ClaimedGiftViewModel(new DialogService(), webApi, shareService);

			SelectedTab = TabTypes.Unclaimed;

			_unclaimedTab = new Tab()
			{
				Title = "12 Gifts Unclaimed",
				SelectedImage = "gift_unclaimed_icon.png",
				UnselectedImage = "gift_claimed_icon.png",
				TabType = TabTypes.Unclaimed,
				IsSelected = true
			};

			_claimedTab = new Tab()
			{
				Title = "6 Gifts claimed",
				SelectedImage = "gift_unclaimed_icon.png",
				UnselectedImage = "gift_claimed_icon.png",
				TabType = TabTypes.Claimed,
				IsSelected = false
			};

			Tabs = new ObservableCollection<Tab>()
			{
				_unclaimedTab,
				_claimedTab
			};
		}

		private UnclaimedGiftViewModel _unclaimedGiftViewModel;
		public UnclaimedGiftViewModel UnclaimedGiftViewModel
		{
			private set { SetProperty(ref _unclaimedGiftViewModel, value);}
			get { return _unclaimedGiftViewModel;}
		}

		private ClaimedGiftViewModel _claimedGiftViewModel;
		public ClaimedGiftViewModel ClaimedGiftViewModel
		{
			private set { SetProperty(ref _claimedGiftViewModel, value); }
			get { return _claimedGiftViewModel; }
		}

		private ObservableCollection<Tab> _tabs;
		public ObservableCollection<Tab> Tabs
		{
			get { return _tabs;}
			set { SetProperty(ref _tabs, value);}
		}

		private MvxCommand<Tab> _selectTabCommand;
		public MvxCommand<Tab> SelectTabCommand
		{
			get
			{
				return _selectTabCommand ?? (_selectTabCommand = new MvxCommand<Tab>(DoSelectTabCommand));
			}
		}

		private TabTypes _selectedTab;
		public TabTypes SelectedTab
		{
			get { return _selectedTab;}
			set 
			{ 
				SetProperty(ref _selectedTab, value);
				UnclaimedGiftViewModel.IsVisible = SelectedTab == TabTypes.Unclaimed;
				ClaimedGiftViewModel.IsVisible = SelectedTab == TabTypes.Claimed;
			}		
		}

		public async void Init()
		{
			await UnclaimedGiftViewModel.Init();
			await ClaimedGiftViewModel.Init();
		}

		private void DoSelectTabCommand(Tab selectedTab)
		{
			foreach (var tab in Tabs)
			{
				if (tab == selectedTab)
				{
					tab.IsSelected = true;
					SelectedTab = tab.TabType;
				}
				else
				{
					tab.IsSelected = false;
				}
			}
		}

		public enum TabTypes
		{
			Unclaimed,
			Claimed
		}

		public class Tab : MvxViewModel
		{
			private string _title;
			public string Title
			{
				get { return _title;}
				set { SetProperty(ref _title, value);}
			}

			public string CurrentImage			
			{
				get { return IsSelected ? SelectedImage : UnselectedImage; }
			}

			private string _selectedImage;
			public string SelectedImage
			{
				get { return _selectedImage;}
				set { SetProperty(ref _selectedImage, value);}
			}

			private string _unselectedImage;
			public string UnselectedImage
			{
				get { return _unselectedImage; }
				set { SetProperty(ref _unselectedImage, value); }
			}

			private bool _isSelected;
			public bool IsSelected
			{
				get { return _isSelected;}
				set 
				{ 
					SetProperty(ref _isSelected, value);
					RaisePropertyChanged(nameof(CurrentImage));
				}
			}

			private TabTypes _tabType;
			public TabTypes TabType
			{
				get { return _tabType; }
				set { SetProperty(ref _tabType, value); }
			}
		}
	}
}

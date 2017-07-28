using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using GodSpeak.Services;
using GodSpeak.Resources;
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
    public class ShareViewModel : CustomViewModel
    {
        private IWebApiService _webApi;
        private IShareService _shareService;

		//private Tab _sendGift;
		private Tab _sendInvitation;
		private Tab _whoYouHaveImpacted;

        public ShareViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMailService mailService, IContactService contactService, IMvxWebBrowserTask webBrowserTask) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _shareService = shareService;

            UnclaimedGiftViewModel = new UnclaimedGiftViewModel (dialogService, hudService, sessionService, webApiService, settingsService, contactService, mailService);
            ClaimedGiftViewModel = new ClaimedGiftViewModel (dialogService, hudService, sessionService, webApiService, settingsService, mailService);
			//SendGiftViewModel = new SendGiftViewModel(dialogService, hudService, sessionService, webApiService, settingsService, webBrowserTask);

            SelectedTab = TabTypes.Unclaimed;

			//_sendGift = new Tab() 
			//{
			//	Title = Text.SendGift,
   //             SelectedImage = "send_gift_selected_icon.png",
   //             UnselectedImage = "send_gift_unselected_icon.png",
   //             TabType = TabTypes.SendGift,
   //             IsSelected = true
			//};

            _sendInvitation = new Tab () {
                Title = Text.SendInvitation,
                SelectedImage = "unclaimed_gift_selected_icon.png",
                UnselectedImage = "unclaimed_gift_unselected_icon.png",
                TabType = TabTypes.Unclaimed,
                IsSelected = true
            };

            _whoYouHaveImpacted = new Tab () {
                Title = Text.WhoYouHaveImpacted,
                SelectedImage = "claimed_gift_selected_icon.png",
                UnselectedImage = "claimed_gift_unselected_icon.png",
                TabType = TabTypes.Claimed,
                IsSelected = false
            };

            Tabs = new ObservableCollection<Tab> ()
            {
				//_sendGift,
                _sendInvitation,
                _whoYouHaveImpacted
            };
        }

		//private SendGiftViewModel _sendGiftViewModel;
		//public SendGiftViewModel SendGiftViewModel
		//{
		//	private set { SetProperty(ref _sendGiftViewModel, value); }
		//	get { return _sendGiftViewModel; }
		//}

        private UnclaimedGiftViewModel _unclaimedGiftViewModel;
        public UnclaimedGiftViewModel UnclaimedGiftViewModel {
            private set { SetProperty (ref _unclaimedGiftViewModel, value); }
            get { return _unclaimedGiftViewModel; }
        }

        private ClaimedGiftViewModel _claimedGiftViewModel;
        public ClaimedGiftViewModel ClaimedGiftViewModel {
            private set { SetProperty (ref _claimedGiftViewModel, value); }
            get { return _claimedGiftViewModel; }
        }

        private ObservableCollection<Tab> _tabs;
        public ObservableCollection<Tab> Tabs {
            get { return _tabs; }
            set { SetProperty (ref _tabs, value); }
        }

        private MvxCommand<Tab> _selectTabCommand;
        public MvxCommand<Tab> SelectTabCommand {
            get {
                return _selectTabCommand ?? (_selectTabCommand = new MvxCommand<Tab> (DoSelectTabCommand));
            }
        }

        private TabTypes _selectedTab;
        public TabTypes SelectedTab {
            get { return _selectedTab; }
            set {
                SetProperty (ref _selectedTab, value);
				//SendGiftViewModel.IsVisible = SelectedTab == TabTypes.SendGift;
                UnclaimedGiftViewModel.IsVisible = SelectedTab == TabTypes.Unclaimed;
                ClaimedGiftViewModel.IsVisible = SelectedTab == TabTypes.Claimed;
            }
        }

        public async void Init (bool comesFromRegisterFlow, TabTypes selectedTab = TabTypes.Unclaimed)
        {
            this.HudService.Show ();

			if (selectedTab == TabTypes.Unclaimed)
			{
				DoSelectTabCommand(_sendInvitation);
			}
			else
			{
                DoSelectTabCommand(_whoYouHaveImpacted);
			}

            await UnclaimedGiftViewModel.Init (comesFromRegisterFlow);
            await ClaimedGiftViewModel.Init (comesFromRegisterFlow);
            this.HudService.Hide ();
        }

        private void DoSelectTabCommand (Tab selectedTab)
        {
            foreach (var tab in Tabs) {
                if (tab == selectedTab) {
                    tab.IsSelected = true;
                    SelectedTab = tab.TabType;
                } else {
                    tab.IsSelected = false;
                }
            }
        }

		public enum TabTypes
		{
			//SendGift,
            Unclaimed,
            Claimed
        }

        public class Tab : MvxViewModel
        {
            private string _title;
            public string Title {
                get { return _title; }
                set { SetProperty (ref _title, value); }
            }

            public string CurrentImage {
                get { return IsSelected ? SelectedImage : UnselectedImage; }
            }

            private string _selectedImage;
            public string SelectedImage {
                get { return _selectedImage; }
                set { SetProperty (ref _selectedImage, value); }
            }

            private string _unselectedImage;
            public string UnselectedImage {
                get { return _unselectedImage; }
                set { SetProperty (ref _unselectedImage, value); }
            }

            private bool _isSelected;
            public bool IsSelected {
                get { return _isSelected; }
                set {
                    SetProperty (ref _isSelected, value);
                    RaisePropertyChanged (nameof (CurrentImage));
                }
            }

            private TabTypes _tabType;
            public TabTypes TabType {
                get { return _tabType; }
                set { SetProperty (ref _tabType, value); }
            }
        }
    }
}

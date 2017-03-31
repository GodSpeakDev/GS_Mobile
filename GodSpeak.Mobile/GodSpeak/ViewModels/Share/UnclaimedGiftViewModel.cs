using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
    public class UnclaimedGiftViewModel : CustomViewModel
    {
        private IWebApiService _webApi;
        private IShareService _shareService;

        private ObservableCollection<CustomViewModel> _pages;
        public ObservableCollection<CustomViewModel> Pages {
            get { return _pages; }
            set { SetProperty (ref _pages, value); }
        }

        private ObservableCollection<ItemCommand<InviteBundle>> _bundles;
        public ObservableCollection<ItemCommand<InviteBundle>> Bundles {
            get { return _bundles; }
            set { SetProperty (ref _bundles, value); }
        }

        private int _selectedPosition;
        public int SelectedPosition {
            get { return _selectedPosition; }
            set { SetProperty (ref _selectedPosition, value); }
        }

        private bool _isVisible;
        public bool IsVisible {
            get { return _isVisible; }
            set { SetProperty (ref _isVisible, value); }
        }

		private MvxCommand<InviteBundle> _tapBundleCommand;
		public MvxCommand<InviteBundle> TapBundleCommand
		{
			get
			{
				return _tapBundleCommand ?? (_tapBundleCommand = new MvxCommand<InviteBundle>(DoTapBundleCommand));
			}
		}

        public UnclaimedGiftViewModel (IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base (dialogService)
        {
            _webApi = webApi;
            _shareService = shareService;
        }

        public async Task Init ()
        {
            var pages = new List<CustomViewModel> ();
            pages.Add (new ShareTemplateViewModel (DialogService, _webApi, _shareService));
            pages.Add (new DidYouKnowTemplateViewModel (DialogService, _webApi, _shareService));
            Pages = new ObservableCollection<CustomViewModel> (pages);

            var bundlesResponse = await _webApi.GetInviteBundles (new GetInviteBundlesRequest ());
            if (bundlesResponse.IsSuccess) {
                Bundles = new ObservableCollection<ItemCommand<InviteBundle>> (
					bundlesResponse.Payload.Select(x => new ItemCommand<InviteBundle>() 
					{
						Item = x,
						TappedCommand = TapBundleCommand
					}));
            }
        }

		private async void DoTapBundleCommand(InviteBundle bundle)
		{
			var userModel1 = bundle;
			await this.DialogService.ShowAlert("In Development", "In Development");
		}
    }
}

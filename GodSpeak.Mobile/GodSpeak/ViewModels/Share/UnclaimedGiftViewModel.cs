using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        private ObservableCollection<InviteBundle> _bundles;
        public ObservableCollection<InviteBundle> Bundles {
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
                Bundles = new ObservableCollection<InviteBundle> (bundlesResponse.Payload);
            }
        }
    }
}

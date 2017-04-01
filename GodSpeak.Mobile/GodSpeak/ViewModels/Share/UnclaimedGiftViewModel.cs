using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using GodSpeak.Services;

namespace GodSpeak
{
    public class UnclaimedGiftViewModel : CustomViewModel
    {        
        private IShareService _shareService;
		private ShareTemplateViewModel _shareTemplateViewModel;
		private DidYouKnowTemplateViewModel _didYouKnowTemplateViewModel;

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

        public UnclaimedGiftViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IShareService shareService) : base (dialogService, hudService, sessionService, webApiService)
        {            
            _shareService = shareService;
        }

        public async Task Init ()
        {
			_shareTemplateViewModel = new ShareTemplateViewModel(DialogService, HudService, SessionService, WebApiService, _shareService);
			_didYouKnowTemplateViewModel = new DidYouKnowTemplateViewModel(DialogService, HudService, SessionService, WebApiService);

            var pages = new List<CustomViewModel> ();
            pages.Add (_shareTemplateViewModel);
            pages.Add (_didYouKnowTemplateViewModel);
            Pages = new ObservableCollection<CustomViewModel> (pages);

			await _didYouKnowTemplateViewModel.Init();

            var bundlesResponse = await WebApiService.GetInviteBundles (new GetInviteBundlesRequest ());
			if (bundlesResponse.IsSuccess)
			{
				Bundles = new ObservableCollection<ItemCommand<InviteBundle>>(
					bundlesResponse.Payload.OrderBy(x => x.Cost).Select(x => new ItemCommand<InviteBundle>()
					{
						Item = x,
						TappedCommand = TapBundleCommand
					}));
			}
			else
			{
				this.HudService.Hide();
				await this.HandleResponse(bundlesResponse);
			}
        }

		private async void DoTapBundleCommand(InviteBundle bundle)
		{
			this.HudService.Show();
			var response = await WebApiService.PurchaseInvite(new PurchaseInviteRequest() 
			{
				Token = SessionService.GetUser().Token,
				Guid = bundle.InviteBundleId
			});
			this.HudService.Hide();

			if (response.IsSuccess)
			{
				await this.DialogService.ShowAlert(response.Title, response.Message);
			}
			else
			{
				await HandleResponse(response);
			}
		}
    }
}

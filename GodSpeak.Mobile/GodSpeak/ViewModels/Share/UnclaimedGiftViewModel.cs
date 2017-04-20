using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using GodSpeak.Services;
using MvvmCross.Plugins.WebBrowser;
using Plugin.InAppBilling;
using GodSpeak.Resources;
using Xamarin.Forms;

namespace GodSpeak
{
    public class UnclaimedGiftViewModel : CustomViewModel
    {
        private IShareService _shareService;
        private ShareTemplateViewModel _shareTemplateViewModel;
        private DonateTemplateViewModel _donateViewModel;
        private DidYouKnowTemplateViewModel _didYouKnowTemplateViewModel;
        private bool _comesFromRegisterFlow;

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
        public MvxCommand<InviteBundle> TapBundleCommand {
            get {
                return _tapBundleCommand ?? (_tapBundleCommand = new MvxCommand<InviteBundle> (DoTapBundleCommand));
            }
        }

        readonly IMvxWebBrowserTask _browserTask;

        public UnclaimedGiftViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMvxWebBrowserTask browserTask) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            this._browserTask = browserTask;
            _shareService = shareService;
        }

        protected override void DoCloseCommand ()
        {
            if (_comesFromRegisterFlow) {
                this.ShowViewModel<HomeViewModel> ();
            } else {
                base.DoCloseCommand ();
            }
        }

        public async Task Init (bool comesFromRegisterFlow)
        {
            _comesFromRegisterFlow = comesFromRegisterFlow;
            _shareTemplateViewModel = new ShareTemplateViewModel (DialogService, HudService, SessionService, WebApiService, SettingsService, _shareService, _browserTask);

            _didYouKnowTemplateViewModel = new DidYouKnowTemplateViewModel (DialogService, HudService, SessionService, WebApiService, SettingsService);
            _donateViewModel = new DonateTemplateViewModel (DialogService, HudService, SessionService, WebApiService, SettingsService, _shareService, _browserTask);
            var pages = new List<CustomViewModel> ();
            pages.Add (_shareTemplateViewModel);
            pages.Add (_donateViewModel);
            pages.Add (_didYouKnowTemplateViewModel);
            Pages = new ObservableCollection<CustomViewModel> (pages);

            await _didYouKnowTemplateViewModel.Init ();
            await _donateViewModel.Init ();
            await _shareTemplateViewModel.Init ();

            var bundlesResponse = await WebApiService.GetInviteBundles (new GetInviteBundlesRequest ());
            if (bundlesResponse.IsSuccess) {
                Bundles = new ObservableCollection<ItemCommand<InviteBundle>> (
                    bundlesResponse.Payload.OrderBy (x => x.NumberOfInvites).Select (x => new ItemCommand<InviteBundle> () {
                        Item = x,
                        TappedCommand = TapBundleCommand
                    }));
            } else {
                this.HudService.Hide ();
                await this.HandleResponse (bundlesResponse);
            }
        }

        private async void DoTapBundleCommand (InviteBundle bundle)
        {
			try
			{
				CrossInAppBilling.Current.InTestingMode = true;

				var connect = await CrossInAppBilling.Current.ConnectAsync();
				if (!connect)
				{
					// Error message
					await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.UnableToConnectToStore);
					return;
				}

				var purchase = await CrossInAppBilling.Current.PurchaseAsync(bundle.AppStoreSku, Plugin.InAppBilling.Abstractions.ItemType.InAppPurchase, "apppayload");
				if (purchase == null)
				{
					// Not Purchased
					await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.UnableToProcessOrder);
					return;
				}

				if (Device.RuntimePlatform == Device.Android)
				{
					var consumedItem = await CrossInAppBilling.Current.ConsumePurchaseAsync(purchase.ProductId, purchase.PurchaseToken);
					 
					// Not Consumed
					if(consumedItem == null)
					{
						await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.UnableToProcessOrder);
						return;      
					}				
				}

				this.HudService.Show();
				var response = await WebApiService.PurchaseInvite(new PurchaseInviteRequest()
				{
					Guid = bundle.InviteBundleId
				});

				if (response.IsSuccess)
				{
					await _shareTemplateViewModel.UpdateGiftsLeftTitle();
					this.HudService.Hide();
					await this.DialogService.ShowAlert(response.Title, response.Message);
				}
				else
				{
					this.HudService.Hide();
					await HandleResponse(response);
				}
			}
			catch (Exception ex)
			{
				// Something bad occurs
			}
			finally
			{
				await CrossInAppBilling.Current.DisconnectAsync();
			} 
        }
    }
}

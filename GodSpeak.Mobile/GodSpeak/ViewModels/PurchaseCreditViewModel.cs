using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Binding;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using System.Linq;
using GodSpeak.Services;

namespace GodSpeak
{
    public class PurchaseCreditViewModel : CustomViewModel
    {
        private IWebApiService _webApi;

        private ObservableCollection<SelectModel<InviteBundle>> _bundles;
        public ObservableCollection<SelectModel<InviteBundle>> Bundles {
            get { return _bundles; }
            set { SetProperty (ref _bundles, value); }
        }

        private MvxCommand<SelectModel<InviteBundle>> tapPurchaseCommand;
        public MvxCommand<SelectModel<InviteBundle>> TapPurchaseCommand {
            get {
                return tapPurchaseCommand ?? (tapPurchaseCommand = new MvxCommand<SelectModel<InviteBundle>> (DoTapPurchaseCommand));
            }
        }

        public PurchaseCreditViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {            
        }

        public async void Init ()
        {
            var response = await WebApiService.GetInviteBundles (new GetInviteBundlesRequest ());

			if (CancellationToken.IsCancellationRequested)
			{
				return;
			}

            if (response.IsSuccess) 
			{
                Bundles = new ObservableCollection<SelectModel<InviteBundle>> (response.Payload.Select (x => new SelectModel<InviteBundle> () {
                    Model = x,
                    Command = TapPurchaseCommand
                }));
            } 
			else 
			{
                await HandleResponse (response);
            }
        }

        private async void DoTapPurchaseCommand (SelectModel<InviteBundle> selectModel)
        {
            var bundle = selectModel.Model;

            var result = await this.DialogService.ShowConfirmation (
                Text.InAppPurchaseTitle,
                string.Format (Text.InAppPurchaseText, bundle.Cost),
                Text.Yes,
                Text.No);

            if (result) 
			{
                var purchaseResponse = await _webApi.PurchaseInvite (new PurchaseInviteRequest () { Guid = bundle.InviteBundleId });

				if (CancellationToken.IsCancellationRequested)
				{
					return;
				}

                if (purchaseResponse.IsSuccess) 
				{
                    await this.DialogService.ShowAlert (Text.PurchaseSuccessTitle, Text.PurchaseSuccessText);
                } 
				else 
				{
                    await HandleResponse (purchaseResponse);
                }
            }
        }
    }
}

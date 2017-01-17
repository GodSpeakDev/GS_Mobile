using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Binding;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class PurchaseCreditViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private ObservableCollection<InviteBundle> _bundles;
		public ObservableCollection<InviteBundle> Bundles
		{
			get { return _bundles; }
			set { SetProperty(ref _bundles, value); }
		}

		private InviteBundle _selectedItem;
		public InviteBundle SelectedItem
		{
			get { return _selectedItem;}
			set 
			{ 
				SetProperty(ref _selectedItem, value);
				TapPurchaseCommand.Execute(_selectedItem);
			}
		}

		private MvxCommand<InviteBundle> tapPurchaseCommand;
		public MvxCommand<InviteBundle> TapPurchaseCommand
		{
			get
			{
				return tapPurchaseCommand ?? (tapPurchaseCommand = new MvxCommand<InviteBundle>(DoTapPurchaseCommand));
			}
		}

		public PurchaseCreditViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
		}

		public async void Init()
		{
			var response = await _webApi.GetInviteBundles(new GetInviteBundlesRequest());

			if (response.IsSuccess)
			{
				Bundles = new ObservableCollection<InviteBundle>(response.Content.Payload);
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private async void DoTapPurchaseCommand(InviteBundle bundle)
		{
			var result = await this.DialogService.ShowConfirmation(
				Text.InAppPurchaseTitle,
				string.Format(Text.InAppPurchaseText, bundle.Cost),
				Text.Yes,
				Text.No);

			if (result)
			{
				var purchaseResponse = await _webApi.PurchaseInvite(new PurchaseInviteRequest() {InviteBundleId=bundle.InviteBundleId});
				if (purchaseResponse.IsSuccess)
				{
					await this.DialogService.ShowAlert(Text.PurchaseSuccessTitle, Text.PurchaseSuccessText);
				}
				else
				{
					await HandleResponse(purchaseResponse);
				}
			}
		}
	}
}

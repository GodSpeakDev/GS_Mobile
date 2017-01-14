using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class RequestInviteCodeViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private WelcomeViewModel _parentViewModel;

		private string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private MvxCommand _requestInviteCodeCommand;
		public MvxCommand RequestInviteCodeCommand
		{
			get
			{
				return _requestInviteCodeCommand ?? (_requestInviteCodeCommand = new MvxCommand(DoRequestInviteCodeCommand));
			}
		}

		private MvxCommand _purchaseCreditCommand;
		public MvxCommand PurchaseCreditCommand
		{
			get
			{
				return _purchaseCreditCommand ?? (_purchaseCreditCommand = new MvxCommand(DoPurchaseCreditCommand));
			}
		}

		public RequestInviteCodeViewModel(WelcomeViewModel parentViewModel, IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
			_parentViewModel = parentViewModel;
		}

		public async void DoRequestInviteCodeCommand()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await this.DialogService.ShowAlert(Text.ErrorPopupTitle, Text.EmailRequiredMessage);
				return;
			}

			var response = await _webApi.RequestInvite(new RequestInviteRequest() {Email=Email});
			if (response.IsSuccess)
			{
				await this.DialogService.ShowAlert(Text.SuccessPopupTitle, Text.RequestInviteSuccessfully);
				Email = string.Empty;
				_parentViewModel.SelectPage<ClaimInviteCodeViewModel>();
			}
			else
			{
				await HandleResponse(response);
			}
		}

		public void DoPurchaseCreditCommand()
		{

		}
	}
}

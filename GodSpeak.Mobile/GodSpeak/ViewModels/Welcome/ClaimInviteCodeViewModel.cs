using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class ClaimInviteCodeViewModel : CustomViewModel
	{
		private WelcomeViewModel _parentViewModel;
		private IWebApiService _webApi;

		private string _inviteCode;
		public string InviteCode
		{
			get { return _inviteCode;}
			set { SetProperty(ref _inviteCode, value);}
		}

		private MvxCommand _claimInviteCodeCommand;
		public MvxCommand ClaimInviteCodeCommand
		{
			get
			{
				return _claimInviteCodeCommand ?? (_claimInviteCodeCommand = new MvxCommand(DoClaimInviteCodeCommand));
			}
		}

		private MvxCommand _dontHaveCodeCommand;
		public MvxCommand DontHaveCodeCommand
		{
			get
			{
				return _dontHaveCodeCommand ?? (_dontHaveCodeCommand = new MvxCommand(DoDontHaveCodeCommand));
			}
		}

		private MvxCommand _alreadyRegisteredCommand;
		public MvxCommand AlreadyRegisteredCommand
		{
			get
			{
				return _alreadyRegisteredCommand ?? (_alreadyRegisteredCommand = new MvxCommand(DoAlreadyRegisteredCommand));
			}
		}

		public ClaimInviteCodeViewModel(WelcomeViewModel parentViewModel, IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_parentViewModel = parentViewModel;
			_webApi = webApi;
		}

		public async void DoClaimInviteCodeCommand()
		{
			if (string.IsNullOrEmpty(InviteCode))
			{
				await this.DialogService.ShowAlert(Text.ErrorPopupTitle, Text.InviteCodeRequiredMessage);
				return;
			}

			var response = await _webApi.ValidateCode(new ValidateCodeRequest() { Code=this.InviteCode});
			if (response.IsSuccess)
			{
				ShowViewModel<RegisterViewModel>();
			}
			else
			{
				if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					var shouldGetACode = await this.DialogService.ShowConfirmation(response.ErrorTitle, response.ErrorMessage, Text.GetACode, Text.TryAgain);
					if (shouldGetACode)
					{
						_parentViewModel.SelectPage<RequestInviteCodeViewModel>();
					}
				}
				else
				{
					await HandleResponse(response);
				}
			}
		}

		public void DoDontHaveCodeCommand()
		{
			_parentViewModel.SelectPage<RequestInviteCodeViewModel>();
		}

		public void DoAlreadyRegisteredCommand()
		{
			ShowViewModel<LoginViewModel>();
		}
	}
}

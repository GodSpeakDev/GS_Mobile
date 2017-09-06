using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
	public class ForgotPasswordViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private MvxCommand _sendInstructionsCommand;
		public MvxCommand SendInstructionsCommand
		{
			get
			{
				return _sendInstructionsCommand ?? (_sendInstructionsCommand = new MvxCommand(DoSendInstructionsCommand));
			}
		}

		public ForgotPasswordViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{			
		}

		private async void DoSendInstructionsCommand()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await this.DialogService.ShowAlert(Text.ErrorPopupTitle, Text.EmailRequiredMessage);
				return;
			}

			var response = await WebApiService.ForgotPassword(new ForgotPasswordRequest() {Email=Email});

			if (CancellationToken.IsCancellationRequested)
			{
				return;
			}

			if (response.IsSuccess)
			{
				await this.DialogService.ShowAlert(Text.SuccessPopupTitle, Text.ForgotPasswordSuccessfully);
			}
			else
			{
				await HandleResponse(response);
			}
		}
	}
}

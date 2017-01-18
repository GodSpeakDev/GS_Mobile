using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class LoginViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private ISessionService _sessionService;

		private string _email;
		public string Email
		{
			get { return _email;}
			set { SetProperty(ref _email, value);}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		private MvxCommand _loginCommand;
		public MvxCommand LoginCommand
		{
			get
			{
				return _loginCommand ?? (_loginCommand = new MvxCommand(DoLoginCommand));
			}
		}

		private MvxCommand _registerCommand;
		public MvxCommand RegisterCommand
		{
			get
			{
				return _registerCommand ?? (_registerCommand = new MvxCommand(DoRegisterCommand));
			}
		}

		private MvxCommand _forgotPasswordCommand;
		public MvxCommand ForgotPasswordCommand
		{
			get
			{
				return _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand(DoForgotPasswordCommand));
			}
		}

		public LoginViewModel(IDialogService dialogService, IWebApiService webApi, ISessionService sessionService) : base(dialogService)
		{
			_sessionService = sessionService;
			_webApi = webApi;
		}

		public void Init()
		{
			Email = "godspeak@gmail.com";
			Password = "123456";
		}

		private async void DoLoginCommand()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await this.DialogService.ShowAlert(Text.ErrorPopupTitle, Text.EmailRequiredMessage);
				return;
			}

			if (string.IsNullOrEmpty(Password))
			{
				await this.DialogService.ShowAlert(Text.ErrorPopupTitle, Text.PasswordRequiredMessage);
				return;
			}

			var response = await _webApi.Login(new LoginRequest() {Email=Email, Password=Password});

			if (response.IsSuccess)
			{
				await _sessionService.SaveUser(response.Content.Payload);
				this.ShowViewModel<HomeViewModel>();
			}
			else
			{
				await HandleResponse(response);	
			}
		}

		private void DoRegisterCommand()
		{
			this.ShowViewModel<RegisterViewModel>();
		}

		private void DoForgotPasswordCommand()
		{
			this.ShowViewModel<ForgotPasswordViewModel>();
		}
	}
}

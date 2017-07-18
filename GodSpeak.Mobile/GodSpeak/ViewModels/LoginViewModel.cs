using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
    public class LoginViewModel : CustomViewModel
    {
        private string _email;
        public string Email {
            get { return _email; }
            set { SetProperty (ref _email, value); }
        }

        private string _password;
        public string Password {
            get { return _password; }
            set { SetProperty (ref _password, value); }
        }

        private MvxCommand _loginCommand;
        public MvxCommand LoginCommand 
		{
            get 
			{
                return _loginCommand ?? (_loginCommand = new MvxCommand (DoLoginCommand));
            }
        }

        private MvxCommand _registerCommand;
        public MvxCommand RegisterCommand 
		{
            get 
			{
                return _registerCommand ?? (_registerCommand = new MvxCommand (DoRegisterCommand));
            }
        }

        private MvxCommand _forgotPasswordCommand;
        public MvxCommand ForgotPasswordCommand 
		{
            get 
			{
                return _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand (DoForgotPasswordCommand));
            }
        }

		private MvxCommand _needRegisterCommand;
		public MvxCommand NeedRegisterCommand
		{
			get
			{
				return _needRegisterCommand ?? (_needRegisterCommand = new MvxCommand(DoNeedRegisterCommand));
			}
		}

        private MvxCommand _goTocCommand;
        public MvxCommand GoTocCommand {
            get {
                return _goTocCommand ?? (_goTocCommand = new MvxCommand(DoGoTocCommand));
            }
        }

        readonly IMvxWebBrowserTask browserTask;

        public LoginViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IMvxWebBrowserTask browserTask) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            this.browserTask = browserTask;
        }

        public void Init ()
        {
			Email = SettingsService.Email;
#if DEBUG
            Email = "Curtisb5673@gmail.com";
			Password = "S3vn!1!";
			//Email = "brett@venadotech.com";
			//Password = "v3nad0";
#endif
        }

        private async void DoLoginCommand ()
        {
            if (string.IsNullOrEmpty (Email)) {
                await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.EmailRequiredMessage);
                return;
            }

            if (string.IsNullOrEmpty (Password)) {
                await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.PasswordRequiredMessage);
                return;
            }
            HudService.Show (Text.Authenticating);
            var response = await WebApiService.Login (new LoginRequest () { Email = Email, Password = Password });
            HudService.Hide ();

            if (response.IsSuccess) 
			{
                await SessionService.SaveUser (response.Payload);
				SettingsService.Token = response.Payload.Token;
                this.ShowViewModel<HomeViewModel> ();
            } 
			else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden) 
			{
                var result = await this.DialogService.ShowMenu (Text.BadRequestTitle, Text.LoginInvalidEmailPassword, Text.TryAgain, Text.ForgotMyPasswordButtonTitle);
                if (result == Text.ForgotMyPasswordButtonTitle) {
                    ForgotPasswordCommand.Execute ();
                }
            } 
			else 
			{
                await HandleResponse (response);
            }
        }

        private void DoRegisterCommand ()
        {
            this.ShowViewModel<RegisterViewModel> ();
        }

        private async void DoForgotPasswordCommand ()
        {
            var input = await this.DialogService.ShowInputPopup (Text.RecoverPasswordTitle, Text.RecoverPasswordText, new InputOptions () { Placeholder = Text.EmailPlaceholder }, Text.SendInstructions, Text.AnonymousNevermind);

            if (input.SelectedButton == Text.SendInstructions) {
                this.HudService.Show ();
                var response = await WebApiService.ForgotPassword (new ForgotPasswordRequest () { Email = input.InputText });
                this.HudService.Hide ();

                if (response.IsSuccess) {
                    await this.DialogService.ShowAlert (Text.RecoverPasswordTitle, string.Format (Text.RecoverPasswordSuccessText, input.InputText), Text.AnonymousSuccessButtonTitle);
                } else {
                    await HandleResponse (response);
                }
            }
        }

		private void DoNeedRegisterCommand()
		{
			this.ShowAndRestoreNavigation<GetStartedViewModel>();
		}

        void DoGoTocCommand ()
        {
            this.browserTask.ShowWebPage ("http://go.givegodspeak.com/TermsConditions");
        }
    }
}

using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
    public class LoginViewModel : CustomViewModel
    {
        private IWebApiService _webApi;
        private ISessionService _sessionService;

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
        public MvxCommand LoginCommand {
            get {
                return _loginCommand ?? (_loginCommand = new MvxCommand (DoLoginCommand));
            }
        }

        private MvxCommand _registerCommand;
        public MvxCommand RegisterCommand {
            get {
                return _registerCommand ?? (_registerCommand = new MvxCommand (DoRegisterCommand));
            }
        }

        private MvxCommand _forgotPasswordCommand;
        public MvxCommand ForgotPasswordCommand {
            get {
                return _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand (DoForgotPasswordCommand));
            }
        }

        readonly IProgressHudService hudService;

        public LoginViewModel (IDialogService dialogService, IWebApiService webApi, ISessionService sessionService, IProgressHudService hudService) : base (dialogService)
        {
            this.hudService = hudService;
            _sessionService = sessionService;
            _webApi = webApi;
        }

        public void Init ()
        {
            Email = "godspeak@gmail.com";
            Password = "123456";
        }

        private async void DoLoginCommand ()
        {

            // Testing Popup
            //var result = await this.DialogService.ShowMenu("Oops", "Sorry, it looks like the email and/or password you submitted are incorrect.", "Try Again", "I Forgot My Password");
            if (string.IsNullOrEmpty (Email)) {
                await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.EmailRequiredMessage);
                return;
            }

            if (string.IsNullOrEmpty (Password)) {
                await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.PasswordRequiredMessage);
                return;
            }
            hudService.Show ();
            var response = await _webApi.Login (new LoginRequest () { Email = Email, Password = Password });
            hudService.Hide ();
            if (response.IsSuccess) {
                await _sessionService.SaveUser (response.Payload);
                this.ShowViewModel<HomeViewModel> ();
            } else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                var result = await this.DialogService.ShowMenu (Text.BadRequestTitle, Text.LoginInvalidEmailPassword, Text.TryAgain, Text.ForgotMyPasswordButtonTitle);
                if (result == Text.ForgotMyPasswordButtonTitle) {
                    ForgotPasswordCommand.Execute ();
                }
            } else {
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
                await this.DialogService.ShowAlert (Text.RecoverPasswordTitle, string.Format (Text.RecoverPasswordSuccessText, input.InputText), Text.AnonymousSuccessButtonTitle);
            }
        }
    }
}

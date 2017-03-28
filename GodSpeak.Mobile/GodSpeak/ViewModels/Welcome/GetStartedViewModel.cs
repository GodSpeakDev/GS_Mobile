using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;

namespace GodSpeak
{
    public class GetStartedViewModel : CustomViewModel
    {
        public Action ShowInviteCodeBox {
            get;
            set;
        }

        public Action ShowGiftCodeSuccessBox {
            get;
            set;
        }

        private string _giftCodeText;
        public string GiftCodeText {
            get { return _giftCodeText; }
            set {
                SetProperty (ref _giftCodeText, value);
                RaisePropertyChanged (nameof (IsSubmitInvideCodeValid));
            }
        }

        public bool IsSubmitInvideCodeValid {
            get {
                var isValid = !string.IsNullOrEmpty (GiftCodeText);
                return isValid;
            }
        }

        readonly IWebApiService webApiService;

        public GetStartedViewModel (IDialogService dialogService, IWebApiService webApiService) : base (dialogService)
        {
            this.webApiService = webApiService;
        }

        public void Init ()
        {
            GiftCodeText = string.Empty;
        }

        private MvxCommand _tapGetStartedCommand;
        public MvxCommand TapGetStartedCommand {
            get {
                return _tapGetStartedCommand ?? (_tapGetStartedCommand = new MvxCommand (DoTapGetStartedCommand));
            }
        }

        private MvxCommand _submitGiftCodeCommand;
        public MvxCommand SubmitGiftCodeCommand {
            get {
                return _submitGiftCodeCommand ?? (_submitGiftCodeCommand = new MvxCommand (DoSubmitGiftCodeCommand));
            }
        }

        private MvxCommand _dontHaveCodeCommand;
        public MvxCommand DontHaveCodeCommand {
            get {
                return _dontHaveCodeCommand ?? (_dontHaveCodeCommand = new MvxCommand (DoDontHaveCodeCommand));
            }
        }

        private MvxCommand _alreadyRegisteredCommand;
        public MvxCommand AlreadyRegisteredCommand {
            get {
                return _alreadyRegisteredCommand ?? (_alreadyRegisteredCommand = new MvxCommand (DoAlreadyRegisteredCommand));
            }
        }

        private MvxCommand _registerCommand;
        public MvxCommand RegisterCommand {
            get {
                return _registerCommand ?? (_registerCommand = new MvxCommand (DoRegisterCommand));
            }
        }

        private void DoTapGetStartedCommand ()
        {
            if (ShowInviteCodeBox != null) {
                ShowInviteCodeBox ();
            }
        }

        protected async void DoSubmitGiftCodeCommand ()
        {
            if (string.IsNullOrEmpty (GiftCodeText)) {
                await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.InviteCodeRequiredMessage);
                return;
            }

            var response = await webApiService.ValidateCode (new ValidateCodeRequest () { Code = this.GiftCodeText });
            if (response.IsSuccess) {
                ShowGiftCodeSuccessBox ();
                //ShowViewModel<RegisterViewModel> ();
            } else {
                //await HandleBadResponse (response);
                await this.DialogService.ShowAlert (response.Title, response.Message);
            }


        }

        private void DoRegisterCommand ()
        {
            this.ShowViewModel<RegisterViewModel> ();
        }

        private async void DoDontHaveCodeCommand ()
        {
            var result = await this.DialogService.ShowInputPopup (Text.AnonymousTitle, Text.AnonymousText, new InputOptions () { Placeholder = Text.AnonymousInputPlaceholder }, Text.AnonymousSubmit, Text.AnonymousNevermind);

            if (result.SelectedButton == Text.AnonymousSubmit) {
                await this.DialogService.ShowAlert (Text.AnonymousSuccessTitle, string.Format (Text.AnonymousSuccessText, result.InputText), Text.AnonymousSuccessButtonTitle);
            }
        }

        private void DoAlreadyRegisteredCommand ()
        {
            this.ShowViewModel<LoginViewModel> ();
        }
    }
}

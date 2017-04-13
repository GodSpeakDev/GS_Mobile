using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;
using System.Threading.Tasks;
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
    public class ShareTemplateViewModel : CustomViewModel
    {
        private IShareService _shareService;

        private MvxCommand _shareWithFriendsCommand;
        public MvxCommand ShareWithFriendsCommand {
            get {
                return _shareWithFriendsCommand ?? (_shareWithFriendsCommand = new MvxCommand (DoShareWithFriendsCommand));
            }
        }

        private MvxCommand _learnMoreCommand;
        public MvxCommand LearnMoreCommand {
            get {
                return _learnMoreCommand ?? (_learnMoreCommand = new MvxCommand (DoLearnMore));
            }
        }

        private string _giftsLeftTitle;
        public string GiftsLeftTitle {
            get { return _giftsLeftTitle; }
            set { SetProperty (ref _giftsLeftTitle, value); }
        }

        readonly IMvxWebBrowserTask browserTask;

        public ShareTemplateViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMvxWebBrowserTask browserTask) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            this.browserTask = browserTask;
            _shareService = shareService;
        }

        public async Task Init ()
        {
            await UpdateGiftsLeftTitle ();
        }

        private async void DoShareWithFriendsCommand ()
        {
            var currentUser = await SessionService.GetUser ();
            if (currentUser.InviteBalance > 0) {
                _shareService.Share (string.Format (Text.ShareText, currentUser.InviteCode, currentUser.FirstName));
            } else {
                await DialogService.ShowAlert (Text.ErrorPopupTitle, Text.ShareWithNoBalance);
            }
        }

        public async Task UpdateGiftsLeftTitle ()
        {
            var profileResponse = await WebApiService.GetProfile ();
            if (profileResponse.IsSuccess) {
                await SessionService.SaveUser (profileResponse.Payload);
                GiftsLeftTitle = string.Format (Text.ShareGiftsLeft, profileResponse.Payload.InviteBalance);
            } else {
                await HandleResponse (profileResponse);
            }
        }

        private async void DoLearnMore ()
        {
            browserTask.ShowWebPage ("http://givegodspeak.com");
        }
    }
}


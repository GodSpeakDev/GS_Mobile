using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;
using System.Threading.Tasks;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Platform.Platform;

namespace GodSpeak
{
    public class ShareTemplateViewModel : CustomViewModel
    {
        private IShareService _shareService;
        private readonly IMvxTrace _tracer;

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

        private bool _shareEnabled;
        public bool ShareEnabled {
            get { return _shareEnabled; }
            set { SetProperty (ref _shareEnabled, value); }
        }

        readonly IMvxWebBrowserTask browserTask;

        public ShareTemplateViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMvxWebBrowserTask browserTask, ILogManager logManager) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            this.browserTask = browserTask;
            _shareService = shareService;
            _tracer = logManager.GetLog();
        }

        public async Task Init ()
        {
            await UpdateGiftsLeftTitle ();
        }

        private async void DoShareWithFriendsCommand ()
        {
            var currentUser = await SessionService.GetUser ();

            _tracer.Trace(MvxTraceLevel.Diagnostic, "Share", "Trying to share. Invite Balance: " + currentUser.InviteBalance);

            if (currentUser.InviteBalance > 0) 
            {
                _tracer.Trace(MvxTraceLevel.Diagnostic, "Share", "Opening Share dialog.");
                _shareService.Share (string.Format (Text.GiftToFriendText, currentUser.InviteCode, currentUser.FirstName));
            } 
            else 
            {
                await DialogService.ShowAlert (Text.ErrorPopupTitle, Text.ShareWithNoBalance);
                _tracer.Trace(MvxTraceLevel.Diagnostic, "Share", "Error Sharing. NO BALANCE");
            }
        }

        public async Task UpdateGiftsLeftTitle ()
        {
            var profileResponse = await WebApiService.GetProfile ();

			if (CancellationToken.IsCancellationRequested)
			{
				return;
			}

            if (profileResponse.IsSuccess) {
                await SessionService.SaveUser (profileResponse.Payload);

                var inviteBalance = profileResponse.Payload.InviteBalance;
                ShareEnabled = inviteBalance > 0;
                if (inviteBalance > 0) {
                    GiftsLeftTitle = string.Format (Text.ShareGiftsLeft, inviteBalance);
                } else {
                    GiftsLeftTitle = Text.PurchaseInvitesText;
                }


            } else {
                await HandleResponse (profileResponse);
            }
        }

        private async void DoLearnMore ()
        {
            browserTask.ShowWebPage ("http://go.givegodspeak.com/GiftiTunes");
        }
    }
}


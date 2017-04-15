using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;
using System.Threading.Tasks;
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
    public class DonateTemplateViewModel : CustomViewModel
    {
        private IShareService _shareService;

        private MvxCommand _donateCommand;
        public MvxCommand DonateCommand {
            get {
                return _donateCommand ?? (_donateCommand = new MvxCommand (DoDonateCommand));
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

        public DonateTemplateViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService, IMvxWebBrowserTask browserTask) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            this.browserTask = browserTask;
            _shareService = shareService;
        }

        public async Task Init ()
        {
            await UpdateGiftsLeftTitle ();
        }

        private async void DoDonateCommand ()
        {
            var currentUser = await SessionService.GetUser ();
            if (currentUser.InviteBalance > 0) {

                HudService.Show ();
                var response = await WebApiService.DonateInvite ();
                HudService.Hide ();

                if (response.IsSuccess) {
                    await DialogService.ShowAlert (response.Title, response.Message);
                } else {
                    await HandleResponse (response);
                }

            } else {
                await DialogService.ShowAlert (Text.ErrorPopupTitle, Text.ShareWithNoBalance);
            }
        }

        public async Task UpdateGiftsLeftTitle ()
        {
            var profileResponse = await WebApiService.GetProfile ();
            if (profileResponse.IsSuccess) {
                await SessionService.SaveUser (profileResponse.Payload);
                GiftsLeftTitle = string.Format (Text.DonateStranger, profileResponse.Payload.InviteBalance);
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


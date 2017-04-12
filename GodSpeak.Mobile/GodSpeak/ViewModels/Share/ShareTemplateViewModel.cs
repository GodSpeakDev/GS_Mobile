using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;
using System.Threading.Tasks;

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

        private MvxCommand _shareWithWorldCommand;
        public MvxCommand ShareWithWorldCommand {
            get {
                return _shareWithWorldCommand ?? (_shareWithWorldCommand = new MvxCommand (DoShareWithWorldCommand));
            }
        }

        private string _giftsLeftTitle;
        public string GiftsLeftTitle {
            get { return _giftsLeftTitle; }
            set { SetProperty (ref _giftsLeftTitle, value); }
        }


        public ShareTemplateViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IShareService shareService) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _shareService = shareService;
        }

        public async Task Init ()
        {
            await UpdateGiftsLeftTitle ();
        }

        private async void DoShareWithFriendsCommand ()
        {
			var currentUser = await SessionService.GetUser();
			if (currentUser.InviteBalance > 0)
			{
				_shareService.Share(string.Format(Text.ShareText, currentUser.InviteCode, currentUser.FirstName));
			}
			else
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.ShareWithNoBalance);
			}
        }

        public async Task UpdateGiftsLeftTitle ()
        {
			var profileResponse = await WebApiService.GetProfile ();
			if (profileResponse.IsSuccess)
			{
				await SessionService.SaveUser(profileResponse.Payload);	
				GiftsLeftTitle = string.Format(Text.ShareGiftsLeft, profileResponse.Payload.InviteBalance);
			}
			else
			{
				await HandleResponse(profileResponse);
			}
        }

        private async void DoShareWithWorldCommand ()
        {
			var currentUser = await SessionService.GetUser();
			if (currentUser.InviteBalance > 0)
			{
				HudService.Show ();
				var response = await WebApiService.DonateInvite();
				HudService.Hide ();

				if (response.IsSuccess) 
				{
			    	await DialogService.ShowAlert(response.Title, response.Message);
				} 
				else 
				{
			    	await HandleResponse(response);
				}
			}
			else
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.ShareWithNoBalance);
			}
        }
    }
}


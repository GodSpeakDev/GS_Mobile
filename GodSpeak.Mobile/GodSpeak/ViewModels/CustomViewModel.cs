using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;
using System.Collections.Generic;
using GodSpeak.Services;

namespace GodSpeak
{
    public class CustomViewModel : MvxViewModel
    {
        public IDialogService DialogService { get; private set; }
		public IProgressHudService HudService { get; private set; }
		public ISessionService SessionService { get; private set; }
		public IWebApiService WebApiService { get; private set; }

        private MvxCommand closeCommand;
        public MvxCommand CloseCommand {
            get { return closeCommand ?? (closeCommand = new MvxCommand (DoCloseCommand)); }
        }

        public CustomViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService)
        {
            DialogService = dialogService;
			HudService = hudService;
			SessionService = sessionService;
			WebApiService = webApiService;
        }

        protected virtual void DoCloseCommand ()
        {
            this.Close (this);
        }

		protected void BackToLoginPage()
		{						
			this.ShowViewModel<LoginViewModel>(presentationBundle:
											   new MvxBundle(new Dictionary<string, string>()
			{
						{"NavigationMode", "RestoreNavigation"}
			}));
		}

        public async Task HandleResponse<T> (ApiResponse<T> response)
        {
            await DialogService.ShowAlert (response.Title ?? Text.ErrorPopupTitle, response.Message ?? Text.ErrorPopupText);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				HudService.Show();
				await WebApiService.Logout(new LogoutRequest() { Token = SessionService.GetUser().Token });
				HudService.Hide();

				BackToLoginPage();
			}            
        }

		protected void ShowAndRestoreNavigation<T>() where T : IMvxViewModel
		{
            this.ShowViewModel<T>(presentationBundle:
											   new MvxBundle(new Dictionary<string, string>()
			{
						{"NavigationMode", "RestoreNavigation"}
			}));
		}
    }
}

﻿using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;
using System.Collections.Generic;
using GodSpeak.Services;
using System.Threading;

namespace GodSpeak
{
    public class CustomViewModel : MvxViewModel
    {
		protected CancellationTokenSource CancellationToken { get; private set; }

        public IDialogService DialogService { get; private set; }
		public IProgressHudService HudService { get; private set; }
		public ISessionService SessionService { get; private set; }
		public IWebApiService WebApiService { get; private set; }
		public ISettingsService SettingsService { get; private set; }

        private MvxCommand closeCommand;
        public MvxCommand CloseCommand {
            get { return closeCommand ?? (closeCommand = new MvxCommand (DoCloseCommand)); }
        }

        public CustomViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService)
        {
            DialogService = dialogService;
			HudService = hudService;
			SessionService = sessionService;
			WebApiService = webApiService;
			SettingsService = settingsService;

			CancellationToken = new CancellationTokenSource();
		}

        protected virtual void DoCloseCommand ()
        {
            CancellationToken.Cancel();
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
			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				HudService.Show();
				await WebApiService.Logout();
				HudService.Hide();

				BackToLoginPage();
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
			{
				await DialogService.ShowAlert(response.Title ?? Text.ErrorPopupTitle, response.Message ?? Text.ErrorPopupRequestTimeout);
			}
			else
			{
				await DialogService.ShowAlert(response.Title ?? Text.ErrorPopupTitle, response.Message ?? Text.ErrorPopupText);
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

		public virtual async Task OnAppearing()
		{
			
		}
    }
}

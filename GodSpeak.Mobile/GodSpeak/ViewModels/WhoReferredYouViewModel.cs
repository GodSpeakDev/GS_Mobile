using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
	public class WhoReferredYouViewModel : CustomViewModel
	{
		private string _email;
		public string Email
		{
			get 
			{
				return _email;
			}
			set 
			{
				SetProperty(ref _email, value);
			}
		}

		private MvxCommand _submitCommand;
		public MvxCommand SubmitCommand
		{
			get
			{
				return _submitCommand ?? (_submitCommand = new MvxCommand(DoSubmitCommand));
			}
		}

		private MvxCommand _useContactsCommand;
		public MvxCommand UseContactsCommand
		{
			get
			{
				return _useContactsCommand ?? (_useContactsCommand = new MvxCommand(DoUseContactsCommand));
			}
		}

		public WhoReferredYouViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
			
		}

		private async void DoSubmitCommand()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await this.DialogService.ShowAlert (Text.ErrorPopupTitle, Text.EmailRequired);
                return;
			}

			HudService.Show();
			var referrerResponse = await WebApiService.SendReferral(new SendReferralRequest() 
			{ 
				ReferringEmailAddress = Email 
			});
			HudService.Hide();

			if (CancellationToken.IsCancellationRequested)
			{
				return;
			}

			if (referrerResponse.IsSuccess)
			{
				this.ShowViewModel<HomeViewModel>(new { comesFromRegisterFlow = true });
			}
			else
			{
				await HandleResponse(referrerResponse);
			}
		}

		private async void DoUseContactsCommand()
		{
			this.ShowViewModel<SelectWhoViewModel>(new { pageMode = SelectWhoViewModel.SelectWhoPages.WhoReferred.ToString() });
		}
	}
}

using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
	public class EmailComposerViewModel : CustomViewModel
	{
		private User _currentUser;

		private string _fromAddress;
		public string FromAddress
		{
			get { return _fromAddress; }
			set { SetProperty(ref _fromAddress, value); }
		}

		private string _subject;
		public string Subject
		{
			get { return _subject; }
			set { SetProperty(ref _subject, value); }
		}

		private string _message;
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		private string[] _toAddresses;

		private MvxCommand _sendCommand;
		public MvxCommand SendCommand
		{
			get
			{
				return _sendCommand ?? (_sendCommand = new MvxCommand(DoSendCommand));
			}
		}

		public EmailComposerViewModel(
			IDialogService dialogService,
			IProgressHudService hudService,
			ISessionService sessionService,
			IWebApiService webApiService,
			ISettingsService settingsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
		}

		public async void Init(string toAddresses)
		{
			_toAddresses = toAddresses.Split(',');
			FromAddress = SettingsService.Email;
			Subject = Text.SendEmailSubject;
			_currentUser = await SessionService.GetUser();

			Message = string.Format(Text.ShareText, _currentUser.FirstName);
		}

		private async void DoSendCommand()
		{
			if (string.IsNullOrEmpty(FromAddress))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.FromAddressRequired);
				return;
			}

			if (string.IsNullOrEmpty(Subject))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.SubjectRequired);
				return;
			}

			if (string.IsNullOrEmpty(Message))
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.MessageRequired);
				return;
			}

			var request = new ShareRequest()
			{
				ToEmailAddresses = _toAddresses,
				FromEmailAddress = FromAddress,
				FromName = _currentUser.FirstName,
				Subject = Subject,
				Message = Message
			};

			HudService.Show();
			var response = await WebApiService.Share(request);
			HudService.Hide();

			if (response.IsSuccess)
			{
				await DialogService.ShowAlert(Text.SuccessPopupTitle, Text.InviteSentSuccessfully);
				this.CloseCommand.Execute();
			}
			else
			{
				await HandleResponse(response);
			}
		}
	}
}

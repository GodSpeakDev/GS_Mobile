using System;
using MvvmCross.Core.ViewModels;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
	public class ShareTemplateViewModel : CustomViewModel
	{
		private IShareService _shareService;

		private MvxCommand _shareWithFriendsCommand;
		public MvxCommand ShareWithFriendsCommand
		{
			get
			{
				return _shareWithFriendsCommand ?? (_shareWithFriendsCommand = new MvxCommand(DoShareWithFriendsCommand));
			}
		}

		private MvxCommand _shareWithWorldCommand;
		public MvxCommand ShareWithWorldCommand
		{
			get
			{
				return _shareWithWorldCommand ?? (_shareWithWorldCommand = new MvxCommand(DoShareWithWorldCommand));
			}
		}

		public ShareTemplateViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IShareService shareService) : base(dialogService, hudService, sessionService, webApiService)
		{			
			_shareService = shareService;
		}

		private async void DoShareWithFriendsCommand()
		{
			var action = await this.DialogService.ShowMenu(
				Text.ShareWithFriendsTitle,
				Text.ShareWithFriendsDescription,
				Text.Individually,
				Text.ViaEmail,
				Text.AnonymousNevermind);

			if (action == Text.Individually)
			{
				_shareService.Share("Share Code Text");
			}
			else if (action == Text.ViaEmail)
			{
				this.ShowViewModel<SelectWhoToSendMailViewModel>();
			}
		}

		private async void DoShareWithWorldCommand()
		{
			await this.DialogService.ShowAlert("In Development", "In Development");
		}
	}
}


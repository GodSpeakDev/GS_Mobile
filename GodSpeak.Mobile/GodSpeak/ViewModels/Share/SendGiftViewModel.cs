using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;
using System.Collections.Generic;
using GodSpeak.Services;
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
	public class SendGiftViewModel : CustomViewModel
	{
		private IMvxWebBrowserTask _browserTask;

		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set { SetProperty(ref _isVisible, value); }
		}

		public SendGiftViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IMvxWebBrowserTask browserTask)
			: base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
			_browserTask = browserTask;
		}

		private MvxCommand _tapLearnHowToPayForwardCommand;
		public MvxCommand TapLearnHowToPayForwardCommand
		{
			get
			{
				return _tapLearnHowToPayForwardCommand ?? (_tapLearnHowToPayForwardCommand = new MvxCommand(DoTapLearnHowToPayForwardCommand));
			}
		}

		private async void DoTapLearnHowToPayForwardCommand()
		{
			_browserTask.ShowWebPage("http://google.com");
		}
	}
}

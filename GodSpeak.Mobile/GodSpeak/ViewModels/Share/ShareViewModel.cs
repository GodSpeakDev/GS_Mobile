using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class ShareViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private IShareService _shareService;

		public ShareViewModel(IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base(dialogService)
		{
			_webApi = webApi;
			_shareService = shareService;

			UnclaimedGiftViewModel = new UnclaimedGiftViewModel(new DialogService(), webApi, shareService);
		}

		private UnclaimedGiftViewModel _unclaimedGiftViewModel;
		public UnclaimedGiftViewModel UnclaimedGiftViewModel
		{
			private set { SetProperty(ref _unclaimedGiftViewModel, value);}
			get { return _unclaimedGiftViewModel;}
		}

		public async void Init()
		{
			await UnclaimedGiftViewModel.Init();
		}
	}
}

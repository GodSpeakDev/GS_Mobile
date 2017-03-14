using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class ShareViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private ObservableCollection<CustomViewModel> _pages;
		public ObservableCollection<CustomViewModel> Pages
		{
			get { return _pages; }
			set { SetProperty(ref _pages, value); }
		}

		private ObservableCollection<InviteBundle> _bundles;
		public ObservableCollection<InviteBundle> Bundles
		{
			get { return _bundles; }
			set { SetProperty(ref _bundles, value); }
		}

		public ShareViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
		}

		public async void Init()
		{
			var pages = new List<CustomViewModel>();
			pages.Add(new ShareTemplateViewModel(DialogService, _webApi));
			pages.Add(new DidYouKnowTemplateViewModel(DialogService, _webApi));
			Pages = new ObservableCollection<CustomViewModel>(pages);

			var bundlesResponse = await _webApi.GetInviteBundles(new GetInviteBundlesRequest());
			if (bundlesResponse.IsSuccess)
			{
				Bundles = new ObservableCollection<InviteBundle>(bundlesResponse.Content.Payload);
			}
		}
	}
}

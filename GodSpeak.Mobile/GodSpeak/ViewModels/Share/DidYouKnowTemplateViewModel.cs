using System;
namespace GodSpeak
{
	public class DidYouKnowTemplateViewModel : CustomViewModel
	{
		private IWebApiService _webApi;
		private IShareService _shareService;

		public DidYouKnowTemplateViewModel(IDialogService dialogService, IWebApiService webApi, IShareService shareService) : base(dialogService)
		{
			_webApi = webApi;
			_shareService = shareService;
		}
	}
}

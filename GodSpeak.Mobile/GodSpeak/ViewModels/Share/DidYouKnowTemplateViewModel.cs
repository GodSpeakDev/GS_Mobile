using System;
using GodSpeak.Services;

namespace GodSpeak
{
	public class DidYouKnowTemplateViewModel : CustomViewModel
	{		
		private IShareService _shareService;

		public DidYouKnowTemplateViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IShareService shareService) : base(dialogService, hudService, sessionService, webApiService)
		{			
			_shareService = shareService;
		}
	}
}

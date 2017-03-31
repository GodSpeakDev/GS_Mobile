using System;
using GodSpeak.Services;

namespace GodSpeak
{
	public class MenuViewModel : CustomViewModel
	{
		public MenuViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService) : base(dialogService, hudService, sessionService, webApiService)
		{
			
		}
	}
}

using System;
using GodSpeak.Services;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class DidYouKnowTemplateViewModel : CustomViewModel
	{
		private string _boxTitle;
		public string BoxTitle
		{
			get { return _boxTitle;}
			set { SetProperty(ref _boxTitle, value);}
		}

		private string _boxMessage;
		public string BoxMessage
		{
			get { return _boxMessage;}
			set { SetProperty(ref _boxMessage, value);}
		}

		public DidYouKnowTemplateViewModel(IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{						
		}

		public async Task Init()
		{			
			var response = await WebApiService.GetDidYouKnow(new TokenRequest() { Token = (await SessionService.GetUser()).Token });

			if (response.IsSuccess)
			{
				BoxTitle = response.Message;
				BoxMessage = response.Payload;
			}
			else
			{
				this.HudService.Hide();
				await HandleResponse(response);
			}
		}
	}
}

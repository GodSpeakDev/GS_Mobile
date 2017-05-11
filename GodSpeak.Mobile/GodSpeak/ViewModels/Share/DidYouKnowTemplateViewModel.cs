using System;
using GodSpeak.Services;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class DidYouKnowTemplateViewModel : CustomViewModel
	{
		private bool _isBalloonVisible;
		public bool IsBalloonVisible
		{
			get { return _isBalloonVisible;}
			set { SetProperty(ref _isBalloonVisible, value);}
		}

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
			IsBalloonVisible = false;
		}

		public async Task Init()
		{			
			var response = await WebApiService.GetDidYouKnow();

			if (response.IsSuccess)
			{
				BoxTitle = response.Message;
				BoxMessage = response.Payload;
				IsBalloonVisible = true;
			}
			else
			{
				this.HudService.Hide();
				await HandleResponse(response);
			}
		}
	}
}

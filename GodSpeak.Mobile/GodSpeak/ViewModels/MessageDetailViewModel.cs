using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;

namespace GodSpeak
{
	public class MessageDetailViewModel : CustomViewModel
	{
		private IShareService _shareService;
		private IWebApiService _webApiService;

		private Message _message;
		public Message Message
		{
			get { return _message;}
			set 
			{
				SetProperty(ref _message, value);
			}
		}

		public MessageDetailViewModel(
			IDialogService dialogService,
			IWebApiService webApiService,
			IShareService shareService) : base(dialogService)
		{
			_shareService = shareService;
			_webApiService = webApiService;
		}

		private MvxCommand _shareCommand;
		public MvxCommand ShareCommand
		{
			get
			{
				return _shareCommand ?? (_shareCommand = new MvxCommand(DoShareCommand));
			}
		}

		public async void Init(string messageId)
		{
			var messageResponse = await _webApiService.GetMessage(new GetMessageRequest() 
			{
				MessageId = new Guid(messageId)
			});

			if (messageResponse.IsSuccess)
			{
				Message = messageResponse.Content.Payload;
			}
			else
			{
				await HandleResponse(messageResponse);
			}
		}

		private void DoShareCommand()
		{
			_shareService.Share(string.Format("{0} - {1}", Message.Text, Message.Author));
		}
	}
}

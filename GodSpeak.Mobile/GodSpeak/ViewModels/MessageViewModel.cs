using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;

namespace GodSpeak
{
	public class MessageViewModel : CustomViewModel
	{
		private IWebApiService _apiService;
		private IShareService _shareService;

		private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
		public ObservableCollection<GroupedCollection<Message, DateTime>> Messages
		{
			get { return _messages;}
			set { SetProperty(ref _messages, value);}
		}

		private Message _selectedItem;
		public Message SelectedItem
		{
			get { return _selectedItem;}
			set { 
				SetProperty(ref _selectedItem, value);
				if (SelectedItem != null)
				{
					TapMessageCommand.Execute(SelectedItem);
				}
			}
		}

		private MvxCommand<Message> _tapMessageCommand;
		public MvxCommand<Message> TapMessageCommand
		{
			get
			{
				return _tapMessageCommand ?? (_tapMessageCommand = new MvxCommand<Message>(DoTapMessageCommand));
			}
		}

		public MessageViewModel(IDialogService dialogService, IWebApiService apiService, IShareService shareService) : base(dialogService)
		{
			_apiService = apiService;
			_shareService = shareService;

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>();
		}

		public async void Init()
		{			
			var messages = await _apiService.GetMessages(new GetMessagesRequest());

			if (messages.IsSuccess)
			{
				Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
				(messages.Content.Messages
				 .GroupBy(x => x.Date)
				 .Select(x => new GroupedCollection<Message, DateTime>(x.Key, x))); 
			}
			else
			{
				await HandleResponse(messages);
			}							
		}

		private void DoTapMessageCommand(Message message)
		{
			_shareService.Share(message.Text);
			SelectedItem = null;
		}
	}
}

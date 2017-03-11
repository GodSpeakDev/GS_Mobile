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
		private IReminderService _reminderService;

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

		private MvxCommand _goToImpactCommand;
		public MvxCommand GoToImpactCommand
		{
			get
			{				
				return _goToImpactCommand ?? (_goToImpactCommand = new MvxCommand(DoGoToImpactCommand));
			}
		}

		private MvxCommand _openDrawerMenuCommand;
		public MvxCommand OpenDrawerMenuCommand
		{
			get
			{
				return _openDrawerMenuCommand ?? (_openDrawerMenuCommand = new MvxCommand(DoOpenDrawerMenuCommand));
			}
		}

		public MessageViewModel(
			IDialogService dialogService, 
			IWebApiService apiService, 
			IReminderService reminderService) : base(dialogService)
		{
			_apiService = apiService;
			_reminderService = reminderService;

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>();
		}

		public async void Init()
		{			
			var messages = await _apiService.GetMessages(new GetMessagesRequest());

			if (messages.IsSuccess)
			{
				Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
				(messages.Content.Messages
				 .Where(x => x.DateTimeToDisplay <= DateTime.Now)
				 .GroupBy(x => x.DateTimeToDisplay.Date)
				 .Select(x => new GroupedCollection<Message, DateTime>(x.Key, x)));
			}
			else
			{
				await HandleResponse(messages);
			}							
		}

		private void DoTapMessageCommand(Message message)
		{
			SelectedItem = null;
			this.ShowViewModel<MessageDetailViewModel>(new {messageId=message.MessageId.ToString()});
		}

		private void DoGoToImpactCommand()
		{
			this.ShowViewModel<ImpactViewModel>();
		}

		private void DoOpenDrawerMenuCommand()
		{
			this.ChangePresentation(new OpenMenuPresentationHint());
		}
	}
}

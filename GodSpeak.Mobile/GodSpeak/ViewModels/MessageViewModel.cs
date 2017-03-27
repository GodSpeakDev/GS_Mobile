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
		private IWebApiService _webApi;
		private IReminderService _reminderService;

		private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
		public ObservableCollection<GroupedCollection<Message, DateTime>> Messages
		{
			get { return _messages;}
			set { SetProperty(ref _messages, value);}
		}

		private ObservableCollection<ImpactDay> _shownImpactDays;
		public ObservableCollection<ImpactDay> ShownImpactDays
		{
			get { return _shownImpactDays; }
			set { SetProperty(ref _shownImpactDays, value); }
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

		private MvxCommand _goToShareCommand;
		public MvxCommand GoToShareCommand
		{
			get
			{
				return _goToShareCommand ?? (_goToShareCommand = new MvxCommand(DoGoToShareCommand));
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
			IWebApiService webApi, 
			IReminderService reminderService) : base(dialogService)
		{
			_webApi = webApi;
			_reminderService = reminderService;

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>();
		}

		public async void Init()
		{			
			var messages = await _webApi.GetMessages(new GetMessagesRequest());

			if (messages.IsSuccess)
			{
				Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
				(messages.Payload.Messages
				 .Where(x => x.DateTimeToDisplay <= DateTime.Now)
				 .GroupBy(x => x.DateTimeToDisplay.Date)
				 .Select(x => new GroupedCollection<Message, DateTime>(x.Key, x)));
			}
			else
			{
				await HandleResponse(messages);
			}	

			var response = await _webApi.GetImpact(new GetImpactRequest());
			if (response.IsSuccess)
			{
				ShownImpactDays = new ObservableCollection<ImpactDay>(response.Payload.Payload);
			}
			else
			{
				await HandleResponse(response);
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

		private void DoGoToShareCommand()
		{
			this.ShowViewModel<ShareViewModel>();
		}

		private void DoOpenDrawerMenuCommand()
		{
			this.ChangePresentation(new OpenMenuPresentationHint());
		}
	}
}

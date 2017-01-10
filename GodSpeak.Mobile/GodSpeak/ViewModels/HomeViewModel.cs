using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSpeak.ViewModels
{
    public class HomeViewModel : CustomViewModel
    {
		private IWebApiService _apiService;

		private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
		public ObservableCollection<GroupedCollection<Message, DateTime>> Messages
		{
			get { return _messages;}
			set { SetProperty(ref _messages, value);}
		}

		public HomeViewModel(IWebApiService apiService)
		{
			_apiService = apiService;
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
    }
}

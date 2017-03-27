using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;
using GodSpeak.Resources;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class MessageSettingsViewModel : CustomViewModel
	{
		private IWebApiService _webApi;

		private ObservableCollection<SettingsGroup> _groups;
		public ObservableCollection<SettingsGroup> Groups
		{
			get { return _groups;}
			set { SetProperty(ref _groups, value);}
		}

		private int _numberOfMessages;
		public int NumberOfMessages
		{
			get { return _numberOfMessages;}
			set { 
				SetProperty(ref _numberOfMessages, value);
				RaisePropertyChanged(nameof(NumberOfMessagesText));
			}
		}

		private TimeSpan _startTime;
		public TimeSpan StartTime
		{
			get { return _startTime;}
			set { SetProperty(ref _startTime, value);}
		}

		private TimeSpan _endTime;
		public TimeSpan EndTime
		{
			get { return _endTime; }
			set { SetProperty(ref _endTime, value); }
		}

		public string NumberOfMessagesText
		{
			get { return string.Format(Text.NumberOfMessagesText, NumberOfMessages); }
		}

		private MvxCommand _plusButtonCommand;
		public MvxCommand PlusButtonCommand
		{
			get
			{
				return _plusButtonCommand ?? (_plusButtonCommand = new MvxCommand(DoPlusButtonCommand));
			}
		}

		private MvxCommand _minusButtonCommand;
		public MvxCommand MinusButtonCommand
		{
			get
			{
				return _minusButtonCommand ?? (_minusButtonCommand = new MvxCommand(DoMinusButtonCommand));
			}
		}

		public MessageSettingsViewModel(IDialogService dialogService, IWebApiService webApi) : base(dialogService)
		{
			_webApi = webApi;
			Groups = new ObservableCollection<SettingsGroup>()
			{
				new SettingsGroup("Allow messages on these days:")
				{
					
				},
				new SettingsGroup("Allow messages from these categories:")
				{
					
				},
			};
		}

		public async void Init()
		{
			await LoadDaysOfWeek();
			await LoadCategories();
		}

		private async Task LoadCategories()
		{
			var categoriesResult = await _webApi.GetCategories(new GetCategoriesRequest());
			if (categoriesResult.IsSuccess)
			{
				var categoryCollection = Groups[1];
				foreach (var item in categoriesResult.Content.Payload)
				{
					categoryCollection.Add(new SettingsItem()
					{
						Title = item.Title,
						IsEnabled = item.Enabled
					});
				}
			}
			else
			{
				await HandleResponse(categoriesResult);
			}
		}

		private async Task LoadDaysOfWeek()
		{
			var messageConfigResult = await _webApi.GetMessageConfig(new GetMessageConfigRequest());
			if (messageConfigResult.IsSuccess)
			{
				var daysCollection = Groups[0];
				foreach (var item in messageConfigResult.Content.Payload.OrderBy(x => x.Weekday))
				{
					daysCollection.Add(new SettingsItem()
					{
						Title = ((DayOfWeek) Enum.Parse(typeof(DayOfWeek), item.Weekday.ToString())).ToString(),
						IsEnabled = item.Enabled
					});
				}

				var day = messageConfigResult.Content.Payload[0];
				NumberOfMessages = day.NumberOfMessages;
				StartTime = day.StartDateTime.ToTimeSpan();
				EndTime = day.EndDateTime.ToTimeSpan();
			}
			else
			{
				await HandleResponse(messageConfigResult);
			}
		}

		private void DoPlusButtonCommand()
		{
			NumberOfMessages += 1;
		}

		private void DoMinusButtonCommand()
		{
			if (NumberOfMessages > 0)
			{
				NumberOfMessages -= 1;
			}
		}
	}

	public class SettingsGroup : ObservableCollection<SettingsItem>
	{
		private string _sectionTitle;
		public string SectionTitle
		{
			get { return _sectionTitle;}
			private set {
				_sectionTitle = value;
			}
		}

		public SettingsGroup(string sectionTitle)
		{
			SectionTitle = sectionTitle;	
		}
	}

	public class SettingsItem : MvxViewModel
	{
		private string _title;
		public string Title
		{
			get { return _title;}
			set { SetProperty(ref _title, value);}
		}

		private bool _isEnabled;
		public bool IsEnabled
		{
			get { return _isEnabled;}
			set { SetProperty(ref _isEnabled, value);}
		}
	}
}

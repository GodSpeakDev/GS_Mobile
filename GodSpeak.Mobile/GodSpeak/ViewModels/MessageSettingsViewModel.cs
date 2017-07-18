using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GodSpeak.Resources;
using System.Threading.Tasks;
using GodSpeak.Services;
using MvvmCross.Plugins.Messenger;

namespace GodSpeak
{
    public class MessageSettingsViewModel : CustomViewModel
    {
		private readonly IMvxMessenger _messenger;
		private SettingsItem _everyDayItem;

        private MvxCommand _goSaveCommand;
        public MvxCommand GoSaveCommand {
            get {
                _goSaveCommand = _goSaveCommand ?? new MvxCommand (DoGoSave);
                return _goSaveCommand;
            }
        }

        public async virtual void DoGoSave ()
        {
			if (!HasAnyChange())
				return;

			HudService.Show (Text.SavingSettings);

            foreach (var setting in User.MessageDayOfWeekSettings) 
			{
                setting.StartTime = StartTime;
                setting.EndTime = EndTime;
                setting.NumOfMessages = NumberOfMessages;
            }

			if (_everyDayItem.IsEnabled)
			{
				foreach (var item in User.MessageDayOfWeekSettings)
				{
					item.Enabled = true;
				}
			}
			else
			{
				var daySettings = Groups[0];
				foreach (var setting in daySettings)
				{
					var day = User.MessageDayOfWeekSettings.FirstOrDefault(s => s.Title == setting.Title);
					if (day != null)
					{
						day.Enabled = setting.IsEnabled;
					}
				}
			}

            var categorySettings = Groups [1];
            foreach (var setting in categorySettings) 
			{
                User.MessageCategorySettings.First (s => s.Title == setting.Title).Enabled = setting.IsEnabled;
            }

            await WebApiService.SaveProfile (User);

            HudService.Hide ();

			_messenger.Publish(new MessageSettingsChangeMessage(this));
        }

		private bool HasAnyChange()
		{
			if (NumberOfMessages != User.MessageDayOfWeekSettings[0].NumOfMessages)
			{
				return true;
			}

			if (StartTime != User.MessageDayOfWeekSettings[0].StartTime)
			{
				return true;
			}

			if (EndTime != User.MessageDayOfWeekSettings[0].EndTime)
			{
				return true;
			}

			var daySettings = Groups[0];
			var previousDaySettings = string.Join(",", User.MessageDayOfWeekSettings.Where(x => x.Enabled).Select(x => x.Title));
			var currentDaySettings = string.Join(",", daySettings.Where(x => x.IsEnabled).Select(x => x.Title));
			if (currentDaySettings == _everyDayItem.Title)
			{
				currentDaySettings = string.Join(",", User.MessageDayOfWeekSettings.Select(x => x.Title));
			}

			if (!previousDaySettings.Equals(currentDaySettings))
			{
				return true;
			}

			var categorySettings = Groups[1];
			var previousCategories = string.Join(",", User.MessageCategorySettings.Where(x => x.Enabled).Select(x => x.Title));
			var currentCategories = string.Join(",", categorySettings.Where(x => x.IsEnabled).Select(x => x.Title.ToString()));

			if (!previousCategories.Equals(currentCategories))
			{
				return true;
			}

			return false;
		}

        private ObservableCollection<SettingsGroup> _groups;
        public ObservableCollection<SettingsGroup> Groups {
            get { return _groups; }
            set { SetProperty (ref _groups, value); }
        }

        private int _numberOfMessages;
        public int NumberOfMessages {
            get { return _numberOfMessages; }
            set {
                SetProperty (ref _numberOfMessages, value);
                RaisePropertyChanged (nameof (NumberOfMessagesText));
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime {
            get { return _startTime; }
            set { SetProperty (ref _startTime, value); }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime {
            get { return _endTime; }
            set { SetProperty (ref _endTime, value); }
        }

        public string NumberOfMessagesText {
            get { return string.Format (Text.NumberOfMessagesText, NumberOfMessages); }
        }

        private MvxCommand _plusButtonCommand;
        public MvxCommand PlusButtonCommand {
            get {
                return _plusButtonCommand ?? (_plusButtonCommand = new MvxCommand (DoPlusButtonCommand));
            }
        }

        private MvxCommand _minusButtonCommand;
        public MvxCommand MinusButtonCommand {
            get {
                return _minusButtonCommand ?? (_minusButtonCommand = new MvxCommand (DoMinusButtonCommand));
            }
        }

        public MessageSettingsViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService, IMvxMessenger messenger) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
			_messenger = messenger;

            Groups = new ObservableCollection<SettingsGroup> ()
            {
                new SettingsGroup("Allow messages on these days:")
                {

                },
                new SettingsGroup("Allow messages from these categories:")
                {

                },
            };
        }

        protected User User;

        public async void Init ()
        {
            HudService.Show (Text.RetrievingSettings);
			var response = await WebApiService.GetProfile ();

			if (response.IsSuccess)
			{
				User = response.Payload;
				StartTime = User.MessageDayOfWeekSettings.First().StartTime;
				EndTime = User.MessageDayOfWeekSettings.First().EndTime;
				NumberOfMessages = User.MessageDayOfWeekSettings.First().NumOfMessages;
				LoadDaysOfWeek(User);
				LoadCategories(User);
				HudService.Hide();
			}
			else
			{
				HudService.Hide();
				await HandleResponse(response);
			}
        }

        private void LoadCategories (User user)
        {

            var categoryCollection = Groups [1];
            foreach (var item in user.MessageCategorySettings)
                categoryCollection.Add (new SettingsItem () {
                    Title = item.Title,
                    IsEnabled = item.Enabled
                });
        }

        private void LoadDaysOfWeek (User user)
        {
            var daysCollection = Groups [0];

			_everyDayItem = new SettingsItem() 
			{
				Title = Text.Everyday,
				IsEnabled = user.MessageDayOfWeekSettings.All(x => x.Enabled)
			};
			_everyDayItem.PropertyChanged += (sender, e) => 
			{
				if (e.PropertyName == nameof(_everyDayItem.IsEnabled))
				{
					RefreshDaysOfWeek(true);
				}
			};

			daysCollection.Add(_everyDayItem);

			RefreshDaysOfWeek();
        }

		private void RefreshDaysOfWeek(bool force = false)
		{
			if (_everyDayItem.IsEnabled)
			{
				foreach (var item in Groups[0].ToList())
				{
					if (item != _everyDayItem)
					{
						Groups[0].Remove(item);
						item.IsEnabled = true;
					}
				}
			}
			else
			{
				var daysCollection = Groups[0];
				foreach (var item in User.MessageDayOfWeekSettings.OrderBy(x => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), x.Title)))
				{
					daysCollection.Add(new SettingsItem()
					{
						Title = item.Title,
						IsEnabled = force ? false : item.Enabled
					});
				}
			}
		}

        private void DoPlusButtonCommand ()
        {
			if (NumberOfMessages < 12)
			{
				NumberOfMessages += 1;
			}
        }

        private void DoMinusButtonCommand ()
        {
            if (NumberOfMessages > 1) {
                NumberOfMessages -= 1;
            }
        }

		protected async override void DoCloseCommand()
		{
			if (Groups.All(group => group.Any(item => item.IsEnabled)))
			{
				base.DoCloseCommand();
			}
			else
			{
				await DialogService.ShowAlert(Text.ErrorPopupTitle, Text.MessageSettingsNotSelected);
			}
		}
    }

    public class SettingsGroup : ObservableCollection<SettingsItem>
    {
        private string _sectionTitle;
        public string SectionTitle {
            get { return _sectionTitle; }
            private set {
                _sectionTitle = value;
            }
        }

        public SettingsGroup (string sectionTitle)
        {
            SectionTitle = sectionTitle;
        }
    }

    public class SettingsItem : MvxViewModel
    {
        private string _title;
        public string Title {
            get { return _title; }
            set { SetProperty (ref _title, value); }
        }

        private bool _isEnabled;
        public bool IsEnabled {
            get { return _isEnabled; }
            set { SetProperty (ref _isEnabled, value); }
        }
    }
}

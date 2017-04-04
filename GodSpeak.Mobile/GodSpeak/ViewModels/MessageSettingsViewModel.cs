﻿using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
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

        private MvxCommand _goSaveCommand;
        public MvxCommand GoSaveCommand {
            get {
                _goSaveCommand = _goSaveCommand ?? new MvxCommand (DoGoSave);
                return _goSaveCommand;
            }
        }

        public async virtual void DoGoSave ()
        {			
            HudService.Show (Text.SavingSettings);

            foreach (var setting in User.MessageDayOfWeekSettings) {
                setting.StartTime = StartTime;
                setting.EndTime = EndTime;
                setting.NumOfMessages = NumberOfMessages;
            }

            var daySettings = Groups [0];
            foreach (var setting in daySettings) {
                User.MessageDayOfWeekSettings.First (s => s.Title == setting.Title).Enabled = setting.IsEnabled;
            }

            var categorySettings = Groups [1];
            foreach (var setting in categorySettings) {
                User.MessageCategorySettings.First (s => s.Title == setting.Title).Enabled = setting.IsEnabled;
            }

            await WebApiService.SaveProfile (User);

            HudService.Hide ();

			_messenger.Publish(new MessageSettingsChangeMessage(this));
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

        public MessageSettingsViewModel (IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, IMvxMessenger messenger) : base (dialogService, hudService, sessionService, webApiService)
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
            var response = await WebApiService.GetProfile (new TokenRequest () { Token = SessionService.GetUser ().Token });

            User = response.Payload;
            StartTime = User.MessageDayOfWeekSettings.First ().StartTime;
            EndTime = User.MessageDayOfWeekSettings.First ().EndTime;
            NumberOfMessages = User.MessageDayOfWeekSettings.First ().NumOfMessages;
            LoadDaysOfWeek (User);
            LoadCategories (User);
            HudService.Hide ();
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
            foreach (var item in user.MessageDayOfWeekSettings)
                daysCollection.Add (new SettingsItem () {
                    Title = item.Title,
                    IsEnabled = item.Enabled
                });
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

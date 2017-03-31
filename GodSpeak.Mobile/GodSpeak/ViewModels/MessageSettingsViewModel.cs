using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;
using GodSpeak.Resources;
using System.Threading.Tasks;
using GodSpeak.Services;

namespace GodSpeak
{
    public class MessageSettingsViewModel : CustomViewModel
    {
        private IWebApiService _webApi;

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

        readonly ISessionService sessionService;
        readonly IProgressHudService hudService;

        public MessageSettingsViewModel (IProgressHudService hudService, ISessionService sessionService, IDialogService dialogService, IWebApiService webApi) : base (dialogService)
        {
            this.hudService = hudService;
            this.sessionService = sessionService;
            _webApi = webApi;
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

        public async void Init ()
        {
            hudService.Show ();
            var response = await _webApi.GetProfile (new TokenRequest () { Token = sessionService.GetUser ().Token });

            var user = response.Payload;

            LoadDaysOfWeek (user);
            LoadCategories (user);
            hudService.Hide ();
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
            NumberOfMessages += 1;
        }

        private void DoMinusButtonCommand ()
        {
            if (NumberOfMessages > 0) {
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

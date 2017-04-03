using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using GodSpeak.Services;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using GodSpeak.Resources;

namespace GodSpeak
{
    public class MessageViewModel : CustomViewModel
    {
        private IReminderService _reminderService;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _token;
        private bool _isAlreadyStarted = false;

        private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
        public ObservableCollection<GroupedCollection<Message, DateTime>> Messages {
            get { return _messages; }
            set { SetProperty (ref _messages, value); }
        }

        private ObservableCollection<ImpactDay> _shownImpactDays;
        public ObservableCollection<ImpactDay> ShownImpactDays {
            get { return _shownImpactDays; }
            set { SetProperty (ref _shownImpactDays, value); }
        }

        private Message _selectedItem;
        public Message SelectedItem {
            get { return _selectedItem; }
            set {
                SetProperty (ref _selectedItem, value);
                if (SelectedItem != null) {
                    TapMessageCommand.Execute (SelectedItem);
                }
            }
        }

        private MvxCommand<Message> _tapMessageCommand;
        public MvxCommand<Message> TapMessageCommand {
            get {
                return _tapMessageCommand ?? (_tapMessageCommand = new MvxCommand<Message> (DoTapMessageCommand));
            }
        }

        private MvxCommand _goToImpactCommand;
        public MvxCommand GoToImpactCommand {
            get {
                return _goToImpactCommand ?? (_goToImpactCommand = new MvxCommand (DoGoToImpactCommand));
            }
        }

        private MvxCommand _goToShareCommand;
        public MvxCommand GoToShareCommand {
            get {
                return _goToShareCommand ?? (_goToShareCommand = new MvxCommand (DoGoToShareCommand));
            }
        }

        private MvxCommand _openDrawerMenuCommand;
        public MvxCommand OpenDrawerMenuCommand {
            get {
                return _openDrawerMenuCommand ?? (_openDrawerMenuCommand = new MvxCommand (DoOpenDrawerMenuCommand));
            }
        }

        public MessageViewModel (
            IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService,
            IReminderService reminderService, IMvxMessenger messenger) : base (dialogService, hudService, sessionService, webApiService)
        {
            _reminderService = reminderService;
            _messenger = messenger;

            Messages = new ObservableCollection<GroupedCollection<Message, DateTime>> ();
        }

        public async void Init ()
        {
            await LoadMessages ();

            _token = _messenger.SubscribeOnMainThread<MessageSettingsChangeMessage> (async (obj) => {
                await LoadMessages ();
            });

            var response = await WebApiService.GetImpact (new GetImpactRequest ());
            if (response.IsSuccess) {
                ShownImpactDays = new ObservableCollection<ImpactDay> (response.Payload.Payload);
            } else {
                await HandleResponse (response);
            }

            _isAlreadyStarted = true;
        }

        private async Task LoadMessages ()
        {
            var messages = new ApiResponse<List<Message>> ();
            if (!_isAlreadyStarted) {
                var shouldShowHud = true;
                Task.Run (async () => {
                    await Task.Delay (2000);
                    if (shouldShowHud) {
                        HudService.Show (Text.RetrievingMessages);
                    }
                });

                messages = await WebApiService.GetMessages (new TokenRequest () {
                    Token = SessionService.GetUser ().Token
                });

                shouldShowHud = false;
                HudService.Hide ();
            } else {
                this.HudService.Show (Text.RetrievingMessages);
                messages = await WebApiService.GetMessages (new TokenRequest () {
                    Token = SessionService.GetUser ().Token
                });
                this.HudService.Hide ();
            }

            if (messages.IsSuccess) {
                Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
                (messages.Payload
                 //.Where (x => x.DateTimeToDisplay <= DateTime.Now)
                 .OrderByDescending (x => x.DateTimeToDisplay)
                 .GroupBy (x => x.DateTimeToDisplay)
                 .Select (x => new GroupedCollection<Message, DateTime> (x.Key, x)));
            } else {
                await HandleResponse (messages);
            }
        }

        private void DoTapMessageCommand (Message message)
        {
            SelectedItem = null;
            this.ShowViewModel<MessageDetailViewModel> (new { messageId = message.Id.ToString () });
        }

        private void DoGoToImpactCommand ()
        {
            this.ShowViewModel<ImpactViewModel> ();
        }

        private void DoGoToShareCommand ()
        {
            this.ShowViewModel<ShareViewModel> ();
        }

        private void DoOpenDrawerMenuCommand ()
        {
            this.ChangePresentation (new OpenMenuPresentationHint ());
        }
    }
}

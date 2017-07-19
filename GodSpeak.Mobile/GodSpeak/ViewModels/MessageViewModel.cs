﻿using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenters;
using GodSpeak.Services;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using GodSpeak.Resources;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace GodSpeak
{
    public class MessageViewModel : CustomViewModel
    {
        private IMessageService _messageService;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _messageSettingsToken;
		private MvxSubscriptionToken _newMessageToken;
		private bool _isAlreadyStarted = false;

        private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
        public ObservableCollection<GroupedCollection<Message, DateTime>> Messages
		{
			get { return _messages; }
			set { SetProperty(ref _messages, value); }
		}

		private bool _shouldShowOverlay;
		public bool ShouldShowOverlay
		{
			get { return _shouldShowOverlay;}
			set { SetProperty(ref _shouldShowOverlay, value);}
		}

		private bool _shouldShowTip;
		public bool ShouldShowTip
		{
			get { return _shouldShowTip; }
			set { SetProperty(ref _shouldShowTip, value); }
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
        public MvxCommand OpenDrawerMenuCommand 
		{
            get 
			{
                return _openDrawerMenuCommand ?? (_openDrawerMenuCommand = new MvxCommand (DoOpenDrawerMenuCommand));
            }
        }

		private MvxCommand _hideOverlayCommand;
		public MvxCommand HideOverlayCommand
		{
			get 
			{
				return _hideOverlayCommand ?? (_hideOverlayCommand = new MvxCommand(DoHideOverlayCommand));
			}
		}

		private MvxCommand _hideTipCommand;
		public MvxCommand HideTipCommand
		{
			get
			{
				return _hideTipCommand ?? (_hideTipCommand = new MvxCommand(DoHideTipCommand));
			}
		}

        public MessageViewModel (
            IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService,
            IMessageService messageService, IMvxMessenger messenger) : base (dialogService, hudService, sessionService, webApiService, settingsService)
        {
            _messageService = messageService;
            _messenger = messenger;

            Messages = new ObservableCollection<GroupedCollection<Message, DateTime>> ();
        }

        public async void Init (bool comesFromRegisterFlow = false)
        {
			ShouldShowOverlay = comesFromRegisterFlow;
			ShouldShowTip = comesFromRegisterFlow;			            

			await LoadMessages ();

            _messageSettingsToken = _messenger.SubscribeOnMainThread<MessageSettingsChangeMessage> (async (obj) => 
			{
				await RefreshSettings();
            });

			_newMessageToken = _messenger.SubscribeOnMainThread<MessageDeliveredMessage> (async(obj) => {
				await ReloadMessages();
            });

			await RefreshImpact();

            _isAlreadyStarted = true;            
        }

		private bool _isShowingHud = false;
        private async Task LoadMessages ()
        {			
            var messages = new List<Message> ();
			if (!_isAlreadyStarted && Xamarin.Forms.Device.RuntimePlatform == "iOS") 
			{				
                Task.Run (async () => 
				{
                    await Task.Delay (1000);                    
                    _isShowingHud = true;
					HudService.Show (Text.RetrievingMessages);
                });

				// Load User
				await InitMessages();

				while (!_isShowingHud)
				{
					
				}

				await Task.Delay(500);
                HudService.Hide ();
            } 
			else 
			{
                this.HudService.Show (Text.RetrievingMessages);

				await InitMessages();

                this.HudService.Hide ();
            }
        }

		private async Task InitMessages()
		{
			await GetUser();

			var messages = await _messageService.GetDeliveredMessages();

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
			(messages
			 .OrderByDescending(x => x.DateTimeToDisplay)
			 .GroupBy(x => x.DateTimeToDisplay.Date)
			 .Select(x => new GroupedCollection<Message, DateTime>(x.Key, x)));
            
			if (!await _messageService.HasUpcomingMessagesInCache())
			{
				if (await _messageService.HasUpcomingMessagesFile())
				{
					// Messages Ran out

					// Load User
					await GetUser();

					var response = await WebApiService.GetProfile();
					var user = response.Payload;

					foreach (var item in user.MessageCategorySettings)
					{
						item.Enabled = item.Title.Contains("Top 100");
					}

					await WebApiService.SaveProfile(user);
				}

				await _messageService.UpdateUpcomingMessages();
				await ReloadMessages();
			}
		}

		private async Task RefreshSettings()
		{
			await Task.Delay(1000);
			await _messageService.UpdateUpcomingMessages();
		}

		private async Task ReloadMessages()
		{
			var user = await GetUser();
			if (user == null)
				return;
			
			var messages = await _messageService.GetDeliveredMessages();

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>
			(messages			 
			 .OrderByDescending(x => x.DateTimeToDisplay)
			 .GroupBy(x => x.DateTimeToDisplay.Date)
			 .Select(x => new GroupedCollection<Message, DateTime>(x.Key, x)));			
		}

		private async Task RefreshImpact()
		{
			var currentUser = await SessionService.GetUser();
			var response = await WebApiService.GetImpact();

            if (response.IsSuccess) 
			{
				if (ShownImpactDays != null)
				{
					foreach (var item in ShownImpactDays.ToList())
					{
						ShownImpactDays.Remove(item);
					}
				}

                ShownImpactDays = new ObservableCollection<ImpactDay> (response.Payload);
            } 
			else 
			{
                await HandleResponse(response);
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
			ShouldShowOverlay = false;
        }

        private void DoOpenDrawerMenuCommand ()
        {
            this.ChangePresentation (new OpenMenuPresentationHint ());
        }

		private void DoHideOverlayCommand()
		{
			ShouldShowTip = false;
			ShouldShowOverlay = false;
		}

		private void DoHideTipCommand()
		{
			ShouldShowTip = false;
		}

		private async Task<User> GetUser()
		{
			var user = await SessionService.GetUser();
			if (user == null)
			{
				await HandleResponse(new ApiResponse<string>() { StatusCode = System.Net.HttpStatusCode.Forbidden });
				return null;
			}
			else
			{
				return user;
			}
		}

		public async override Task OnAppearing()
		{
			if (_isAlreadyStarted)
			{
				await InitMessages();
				await RefreshImpact();
			}
		}
    }
}

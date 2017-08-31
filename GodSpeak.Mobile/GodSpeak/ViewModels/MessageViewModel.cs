using MvvmCross.Core.ViewModels;
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
using MvvmCross.Plugins.WebBrowser;

namespace GodSpeak
{
	public class MessageViewModel : CustomViewModel
	{
		private ISmsService _smsService;
		private IMailService _mailService;
		private IMessageService _messageService;
		private IMvxMessenger _messenger;
		private MvxSubscriptionToken _messageSettingsToken;
		private MvxSubscriptionToken _newMessageToken;
		private MvxSubscriptionToken _openActionMenuToken;
		private IMvxWebBrowserTask _browserTask;
		private bool _isAlreadyStarted = false;

		public Action<MenuItem> HighlightHint
		{
			get; set;
		}

		private ObservableCollection<GroupedCollection<Message, DateTime>> _messages;
		public ObservableCollection<GroupedCollection<Message, DateTime>> Messages
		{
			get { return _messages; }
			set { SetProperty(ref _messages, value); }
		}

		private bool _shouldShowOverlay;
		public bool ShouldShowOverlay
		{
			get { return _shouldShowOverlay; }
			set { SetProperty(ref _shouldShowOverlay, value); }
		}

		private bool _shouldShowTip;
		public bool ShouldShowTip
		{
			get { return _shouldShowTip; }
			set { SetProperty(ref _shouldShowTip, value); }
		}

		private bool _isHelpMode = false;
		public bool IsHelpMode
		{
			get { return _isHelpMode; }
			set { SetProperty(ref _isHelpMode, value); }
		}

		private bool _isActionMenuOpened;
		public bool IsActionMenuOpened
		{
			get { return _isActionMenuOpened; }
			set { SetProperty(ref _isActionMenuOpened, value); }
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
			get { return _selectedItem; }
			set
			{
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

		private MvxCommand _openActionMenuCommand;
		public MvxCommand OpenActionMenuCommand
		{
			get
			{
				return _openActionMenuCommand ?? (_openActionMenuCommand = new MvxCommand(() =>
				{
					IsActionMenuOpened = true;
					ShouldShowOverlay = true;
				}));
			}
		}

		private MvxCommand _closeActionMenuCommand;
		public MvxCommand CloseActionMenuCommand
		{
			get
			{
				return _closeActionMenuCommand ?? (_closeActionMenuCommand = new MvxCommand(() =>
				{
					if (!IsHelpMode)
					{
						IsActionMenuOpened = false;
						ShouldShowOverlay = false;
					}
					else
					{
						ToggleHelpMode();
					}
				}));
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

		public MvxCommand _giftIphoneCommand;
		public MvxCommand GiftIphoneCommand
		{
			get
			{
				return _giftIphoneCommand ?? (_giftIphoneCommand = new MvxCommand(() =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.GiftToIphoneUser));
					}
					else
					{
						_browserTask.ShowWebPage("http://go.givegodspeak.com/GiftiTunes");
						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _giftAndroidCommand;
		public MvxCommand GiftAndroidCommand
		{
			get
			{
				return _giftAndroidCommand ?? (_giftAndroidCommand = new MvxCommand(async () =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.GiftToAndroidUser));
					}
					else
					{
						var user = await SessionService.GetUser();
						_browserTask.ShowWebPage("http://go.givegodspeak.com/GiftAndroid?emailAddress=" + user.Email);
						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _giftChurchCommand;
		public MvxCommand GiftChurchCommand
		{
			get
			{
				return _giftChurchCommand ?? (_giftChurchCommand = new MvxCommand(() =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.GiftToChurch));
					}
					else
					{
						_mailService.SendMail(new string[] { "curtis@givegodspeak.com" }, null, null, "I Want to Share with My Church", "Hi,\rI'm interested in learning more about how to share with my fellow church members");
						//_browserTask.ShowWebPage(string.Format("http://go.givegodspeak.com/SignUp/{0}", (await SessionService.GetUser()).InviteCode));
						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _dontKnowCommand;
		public MvxCommand DontKnowCommand
		{
			get
			{
				return _dontKnowCommand ?? (_dontKnowCommand = new MvxCommand(async () => {

					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.DoNotKnowPlatform));
					}
					else
					{
						var response = await DialogService.ShowMenu(Text.ReachOutTitle, Text.ReachOutText, Text.ReachOutViaEmail, Text.ReachOutViaTextMessage, Text.AnonymousNevermind);

						if (response == Text.ReachOutViaEmail)
						{
							_mailService.SendMail(new string[] {}, body: Text.ReachOutMessageBody, subject: Text.ReachOutMessageSubject);
						}
						else if (response == Text.ReachOutViaTextMessage)
						{
							_smsService.SendMessage(Text.ReachOutMessageBody);
						}

						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _followFriendsCommand;
		public MvxCommand FollowFriendsCommand
		{
			get
			{
				return _followFriendsCommand ?? (_followFriendsCommand = new MvxCommand(() =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.FollowUpWithFriends));
					}
					else
					{
						this.ShowViewModel<ShareViewModel>(new { selectedTab = ShareViewModel.TabTypes.Claimed });
						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _tellFriendsCommand;
		public MvxCommand TellFriendsCommand
		{
			get
			{
				return _tellFriendsCommand ?? (_tellFriendsCommand = new MvxCommand(() =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.TellFriendsAboutGodSpeak));
					}
					else
					{
						this.ShowViewModel<ShareViewModel>(new { selectedTab = ShareViewModel.TabTypes.Unclaimed });
						CloseActionMenuCommand.Execute();
					}
				}));
			}
		}

		public MvxCommand _helpCommand;
		public MvxCommand HelpCommand
		{
			get
			{
				return _helpCommand ?? (_helpCommand = new MvxCommand(() =>
				{
					if (IsHelpMode)
					{
						HightlightHintItem(MenuItems.First(x => x.Title == Text.Help));
					}
					else
					{
						ToggleHelpMode();
					}
				}));
			}
		}

		private ObservableCollection<MenuItem> _menuItems;
		public ObservableCollection<MenuItem> MenuItems
		{
			get { return _menuItems; }
			set { SetProperty(ref _menuItems, value); }
		}

		public MessageViewModel(
			IDialogService dialogService, IProgressHudService hudService, ISessionService sessionService, IWebApiService webApiService, ISettingsService settingsService,
			IMessageService messageService, IMvxMessenger messenger, IMvxWebBrowserTask browserTask, IMailService mailService, ISmsService smsService) : base(dialogService, hudService, sessionService, webApiService, settingsService)
		{
			_smsService = smsService;
			_mailService = mailService;
			_messageService = messageService;
			_messenger = messenger;
			_browserTask = browserTask;

			Messages = new ObservableCollection<GroupedCollection<Message, DateTime>>();
			InitMenu();
		}

		private void InitMenu()
		{
			var menuItems = new List<MenuItem>();

			menuItems.Add(new MenuItem()
			{
				Title = Text.Cancel,
				Image = "close_button_icon.png",
				IsHighlighted = true,
				Hint = Text.CancelHint,
				HintMode = MenuItem.HintModes.Inline,
				Command = CloseActionMenuCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.GiftToIphoneUser,
				Image = "iphone.png",
				IsHighlighted = true,
				Hint = Text.GiftIosHint,
				Command = GiftIphoneCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.GiftToAndroidUser,
				Image = "android.png",
				IsHighlighted = true,
				Hint = Text.GiftDroidHint,
				Command = GiftAndroidCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.DoNotKnowPlatform,
				Image = "phone_type.png",
				IsHighlighted = true,
				Hint = Text.PhoneTypeHint,
				Command = DontKnowCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.GiftToChurch,
				Image = "church.png",
				IsHighlighted = true,
				Hint = Text.GiftChurchHint,
				Command = GiftChurchCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.FollowUpWithFriends,
				Image = "follow_friends.png",
				IsHighlighted = true,
				Hint = Text.FollowFriendHint,
				Command = FollowFriendsCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.TellFriendsAboutGodSpeak,
				Image = "spread_word.png",
				IsHighlighted = true,
				Hint = Text.SpreadWordHint,
				Command = TellFriendsCommand
			});

			menuItems.Add(new MenuItem()
			{
				Title = Text.Help,
				Image = "question_mark.png",
				IsHighlighted = true,
				Hint = Text.HelpHint,
				HintMode = MenuItem.HintModes.Inline,
				Command = HelpCommand
			});

			MenuItems = new ObservableCollection<MenuItem>(menuItems);
		}

		public async void Init(bool comesFromRegisterFlow = false)
		{
			ShouldShowOverlay = comesFromRegisterFlow;
			ShouldShowTip = comesFromRegisterFlow;

			await LoadMessages();

			_messageSettingsToken = _messenger.SubscribeOnMainThread<MessageSettingsChangeMessage>(async (obj) =>
			{
				await RefreshSettings();
			});

			_newMessageToken = _messenger.SubscribeOnMainThread<MessageDeliveredMessage>(async (obj) => {
				await ReloadMessages();
			});

			_openActionMenuToken = _messenger.SubscribeOnMainThread<ShowActionMenuMessage>((obj) => {
				OpenActionMenuCommand.Execute();
			});

			await RefreshImpact();

			_isAlreadyStarted = true;
		}

		private bool _isShowingHud = false;
		private async Task LoadMessages()
		{
			var messages = new List<Message>();
			if (!_isAlreadyStarted && Xamarin.Forms.Device.RuntimePlatform == "iOS")
			{
				Task.Run(async () =>
				{
					await Task.Delay(1000);
					_isShowingHud = true;
					HudService.Show(Text.RetrievingMessages);
				});

				// Load User
				await InitMessages();

				while (!_isShowingHud)
				{

				}

				await Task.Delay(500);
				HudService.Hide();
			}
			else
			{
				this.HudService.Show(Text.RetrievingMessages);

				await InitMessages();

				this.HudService.Hide();
			}
		}

		private void ToggleHelpMode()
		{
			IsHelpMode = !IsHelpMode;

			if (IsHelpMode)
			{
				foreach (var menuItem in MenuItems)
				{
					if (menuItem.Title != Text.Cancel && menuItem.Title != Text.Help)
					{
						menuItem.IsHighlighted = false;
						menuItem.ShowHint = false;
					}
				}

				HightlightHintItem(MenuItems.First(x => x.Title == Text.Help));
				MenuItems.First(x => x.Title == Text.Cancel).ShowHint = true;
			}
			else
			{
				foreach (var menuItem in MenuItems)
				{
					if (menuItem.Title != Text.Cancel)
					{
						menuItem.IsHighlighted = true;
						menuItem.ShowHint = false;
					}
				}

				MenuItems.First(x => x.Title == Text.Cancel).ShowHint = false;
			}
		}

		private void HightlightHintItem(MenuItem item)
		{
			foreach (var menuItem in MenuItems)
			{
				if (menuItem.Title != Text.Cancel)
				{
					menuItem.IsHighlighted = menuItem == item;
					menuItem.ShowHint = menuItem == item;
				}
			}

			HighlightHint?.Invoke(item);
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

				ShownImpactDays = new ObservableCollection<ImpactDay>(response.Payload);
			}
			else
			{
				await HandleResponse(response);
			}
		}

		private void DoTapMessageCommand(Message message)
		{
			SelectedItem = null;
			this.ShowViewModel<MessageDetailViewModel>(new { messageId = message.Id.ToString() });
		}

		private void DoGoToImpactCommand()
		{
			this.ShowViewModel<ImpactViewModel>();
		}

		private void DoGoToShareCommand()
		{
			this.ShowViewModel<ShareViewModel>();
			ShouldShowOverlay = false;
		}

		private void DoOpenDrawerMenuCommand()
		{
			this.ChangePresentation(new OpenMenuPresentationHint());
		}

		private void DoHideOverlayCommand()
		{
			ShouldShowTip = false;
			ShouldShowOverlay = false;
			IsActionMenuOpened = false;
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

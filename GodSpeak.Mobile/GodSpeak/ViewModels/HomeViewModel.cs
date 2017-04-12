using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;
using GodSpeak.Resources;
using GodSpeak.Services;

namespace GodSpeak
{
    public class HomeViewModel : MvxMasterDetailViewModel<MessageViewModel>
    {
        private MvxCommand _shareCommand;
        public ICommand ShareCommand {
            get {
                return _shareCommand ?? (_shareCommand = new MvxCommand (() => {
                    this.ChangePresentation (new CloseMenuPresentationHint ());
                    this.ShowViewModel<ShareViewModel> ();
                }));
            }
        }

        private MvxCommand _goLogoutCommand;
        public MvxCommand LogoutCommand {
            get {
                _goLogoutCommand = _goLogoutCommand ?? new MvxCommand (DoGoLogoutCommand);
                return _goLogoutCommand;
            }
        }

        public virtual async void DoGoLogoutCommand ()
        {
            hudService.Show ();
			await webService.Logout ();
            hudService.Hide ();

			this.settingsService.Token = null;
            this.ChangePresentation (new CloseMenuPresentationHint ());
            this.ShowViewModel<LoginViewModel> (presentationBundle:
                                               new MvxBundle (new Dictionary<string, string> ()
            {
                        {"NavigationMode", "RestoreNavigation"}
            }));
        }

        private MvxCommand _messageSettingsCommand;
        public MvxCommand MessageSettingsCommand {
            get {
                return _messageSettingsCommand ?? (_messageSettingsCommand = new MvxCommand (() => {
                    this.ChangePresentation (new CloseMenuPresentationHint ());
                    this.ShowViewModel<MessageSettingsViewModel> ();
                }));
            }
        }

        private MvxCommand _myProfileCommand;
        public MvxCommand MyProfileCommand {
            get {
                return _myProfileCommand ?? (_myProfileCommand = new MvxCommand (() => {
                    this.ChangePresentation (new CloseMenuPresentationHint ());
                    this.ShowViewModel<MyProfileViewModel> ();
                }));
            }
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems {
            get { return _menuItems; }
            set { SetProperty (ref _menuItems, value); }
        }


        private MvxCommand _feedbackCommand;
        public MvxCommand FeedbackCommand {
            get {
                _feedbackCommand = _feedbackCommand ?? new MvxCommand (DoGonotset);
                return _feedbackCommand;
            }
        }

        private MvxCommand<MenuItem> _menuItemSelectedCommand;
        public MvxCommand<MenuItem> MenuItemSelectedCommand {
            get {
                _menuItemSelectedCommand = _menuItemSelectedCommand ?? new MvxCommand<MenuItem> (DoMenuItemSelectedCommand);
                return _menuItemSelectedCommand;
            }
        }

        public virtual void DoGonotset ()
        {
            feedbackService.OpenFeedbackDialog ();
        }

        readonly IFeedbackService feedbackService;
        readonly IWebApiService webService;
        readonly IProgressHudService hudService;
        readonly ISessionService sessionService;
		readonly ISettingsService settingsService;

        public HomeViewModel (ISessionService sessionService, IFeedbackService feedbackService, IWebApiService webService, IProgressHudService hudService, ISettingsService settingsService)
        {
            this.sessionService = sessionService;
            this.hudService = hudService;
            this.webService = webService;
            this.feedbackService = feedbackService;
			this.settingsService = settingsService;

            MenuItems = new ObservableCollection<MenuItem> ()
            {
                new MenuItem()
                {
                    Command = MessageSettingsCommand,
                    Title = Text.MenuMessageSettings,
                    Image = "settings_icon.png"
                },
                new MenuItem()
                {
                    Command = MyProfileCommand,
                    Title = Text.MyProfileSettings,
                    Image = "profile_icon.png"
                },
                new MenuItem()
                {
                    Command = LogoutCommand,
                    Title = Text.LogoutSettings,
                    Image = "logout_icon.png"
                },
                new MenuItem()
                {
                    Command = FeedbackCommand,
                    Title = Text.SubmitBugFeedback,
                    Image = "bug_icon.png"
                }

            };
        }

        private void DoMenuItemSelectedCommand (MenuItem menuItem)
        {
            menuItem.Command.Execute ();
        }
    }
}


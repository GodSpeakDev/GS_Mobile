using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using Foundation;
using UIKit;
using System;
using HockeyApp.iOS;
using MvvmCross.Plugins.Messenger;
using Google.Maps;
using System.Linq;

namespace GodSpeak.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			MapServices.ProvideAPIKey("AIzaSyC0GOJSGqQ5r3yDolVrXMJxQse7RCZTWaE");

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("f63257a3cdd24189a417326e041a318f");
			manager.StartManager();
			manager.Authenticator.AuthenticateInstallation();

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var setup = new Setup(this, Window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			Window.MakeKeyAndVisible();

			CustomKeyboardOverlapRenderer.Init();

			//if ([UIDevice currentDevice].systemVersion.floatValue >= 8.0)
			//{
			//	UIUserNotificationSettings* settings = [UIUserNotificationSettings settingsForTypes: UIUserNotificationTypeBadge | UIUserNotificationTypeAlert | UIUserNotificationTypeSound categories: nil];
			// 			[application registerUserNotificationSettings:settings]; 
			//		}

			var version = new Version(UIDevice.CurrentDevice.SystemVersion);
			if (version.Major > 8)
			{
				var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
				app.RegisterUserNotificationSettings(settings);
			}

			//setting global vars
			App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
			App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

			UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIKit.UIApplication.BackgroundFetchIntervalMinimum);

			return true;
		}

		public override void WillEnterForeground(UIApplication application)
		{
			base.WillEnterForeground(application);
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		public async override void OnActivated(UIApplication application)
		{
#if !DEBUG
			var uiLocalNotificationLimit = 64;
			if (UIApplication.SharedApplication.ScheduledLocalNotifications.ToList().Count < 64)
			{
				var messageService = Mvx.Resolve<IMessageService>();
				var reminderService = Mvx.Resolve<IReminderService>();

				var upcomingMessages = (await messageService.GetUpcomingMessages()).Where(x => x.DateTimeToDisplay > DateTime.Now.AddMinutes(2));

				foreach (var upcomingMessage in upcomingMessages)
				{					
					reminderService.SetMessageReminder(upcomingMessage);					

					if (UIApplication.SharedApplication.ScheduledLocalNotifications.Length == uiLocalNotificationLimit)
					{
						break;
					}
				}
			}
#endif
		}

        public override void ReceivedLocalNotification (UIApplication application, UILocalNotification notification)
        {
            var alert = new UIAlertView ("God Speak", notification.AlertBody, null, "Ok");
			alert.Show();

			Mvx.Resolve<IMvxMessenger>().Publish(new MessageDeliveredMessage(this));	
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
    }
}

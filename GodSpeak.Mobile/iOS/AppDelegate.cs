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

            Log("**** FINISHED LAUNCHING ****");

			return true;
		}

		public override void WillEnterForeground(UIApplication application)
		{
			base.WillEnterForeground(application);
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

			Log("**** FOREGROUND ****");
			App.RefreshCurrentPage();
		}

		public override void DidEnterBackground(UIApplication application)
		{
			base.DidEnterBackground(application);
            Log("**** BACKGROUND ****");
		}

		public override void WillTerminate(UIApplication application)
		{
			base.WillTerminate(application);
            Log("**** TERMINATE ****");
		}

		private void Log(string text)
		{
			if (Mvx.CanResolve<ILoggingService>())
			{
				ILoggingService loggingService;
				var hasLogging = Mvx.TryResolve<ILoggingService>(out loggingService);
				if (hasLogging)
				{
					loggingService.Trace(text);
				}
			}
		}

		public async override void OnActivated(UIApplication application)
		{
#if !DEBUG
			if (UIApplication.SharedApplication.ScheduledLocalNotifications.ToList().Count < ReminderService.LocalNotificationLimit)
			{
				var messageService = Mvx.Resolve<IMessageService>();
				await messageService.SetReminders();
			}
#endif
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			if (notification.AlertBody == GodSpeak.Resources.Text.OpenGodSpeakReminder)
			{
				UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
				return;
			}

			var alert = new UIAlertView("God Speak", notification.AlertBody, null, "Ok");
			alert.Show();

			Mvx.Resolve<IMvxMessenger>().Publish(new MessageDeliveredMessage(this));

			var azureApi = Mvx.Resolve<IWebApiService>();

			if (azureApi != null)
			{
				var messageTitle = notification.UserInfo.ValueForKey(new Foundation.NSString(ReminderService.MessageTitleKey));
				azureApi.RecordMessageDelivered(new RecordMessageDeliveredRequest() 
				{
					VerseCode = messageTitle.ToString(),
					DateDelivered=DateTime.Now
				});
			}

			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
    }
}

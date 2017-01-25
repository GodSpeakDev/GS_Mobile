﻿using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using Foundation;
using UIKit;
using System;

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
			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			var setup = new Setup(this, Window);
			setup.Initialize();

			var startup = Mvx.Resolve<IMvxAppStart>();
			startup.Start();

			Window.MakeKeyAndVisible();

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

			return true;
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			var alert = new UIAlertView("Reminder", notification.AlertBody, null, "Ok");

			alert.Show();
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}
	}
}

using System;

using Xamarin.Forms.Platform.Android;

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using System.Net;
using HockeyApp.Android;

using Plugin.Permissions;
using Plugin.InAppBilling;

using Android.Content.Res;
using MvvmCross.Droid.Platform;

namespace GodSpeak.Droid
{
    [Activity (Theme = "@style/AppTheme", Label = "GodSpeak", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan, LaunchMode=LaunchMode.SingleTop)]
    public class MainActivity : FormsApplicationActivity
    {
		public const int RequestReadContacts = 0;
        public static bool IsForeground { get; set; }

        protected override void OnCreate (Bundle bundle)
        {
            CrashManager.Register (this, "da7c75f8b96b4b3abc8569120d7e1b3a");

			FeedbackManager.Register(this, "da7c75f8b96b4b3abc8569120d7e1b3a");

            Xamarin.Forms.Forms.Init (this, bundle);
            Xamarin.FormsMaps.Init (this, bundle);
			global::Xamarin.Forms.Forms.SetTitleBarVisibility(Xamarin.Forms.AndroidTitleBarVisibility.Never);

			Android.Gms.Maps.MapsInitializer.Initialize(ApplicationContext);

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => {
                        return true;
                    };

            base.OnCreate (bundle);

            var mvxFormsApp = new FormsApp ();
            LoadApplication (mvxFormsApp);

			var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
			setup.EnsureInitialized();

            var presenter = Mvx.Resolve<IMvxViewPresenter> () as MvxFormsDroidMasterDetailPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart> ().Start ();

            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);            
        }

        protected override void OnStart ()
        {
            base.OnStart ();
            IsForeground = true;

            Log("**** START ****");
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            IsForeground = true;

            Log("**** RESUME ****");
			App.RefreshCurrentPage();
        }

        protected override void OnStop ()
        {
            base.OnStop ();

            Log("**** STOP ****");
            IsForeground = false;
        }

        protected override void OnPause ()
        {
            base.OnPause ();
            IsForeground = false;

            Log("**** PAUSE ****");
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

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (requestCode == RequestReadContacts)
			{
				var contactService = Mvx.Resolve<IContactService>() as ContactsService;
				contactService.OnRequestPermissionsResult(grantResults[0] == Permission.Granted);
			}
			else
			{
				PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
            InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
		}

		protected override void AttachBaseContext(Context @base)
		{
			base.AttachBaseContext(@base);
			var configuration = new Configuration(
				// Copy the original configuration so it isn't lost.
				@base.Resources.Configuration
        	);
			configuration.FontScale = 1.0f;

			ApplyOverrideConfiguration(configuration);
		}
    }
}

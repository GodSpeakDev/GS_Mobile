using System;

using Xamarin.Forms.Platform.Android;

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Forms.Presenter.Droid;
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

namespace GodSpeak.Droid
{
    [Activity (Theme = "@style/AppTheme", Label = "MvxFormsApplicationActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : FormsApplicationActivity
    {
		public const int RequestReadContacts = 0;
        public static bool IsForeground { get; set; }

        protected override void OnCreate (Bundle bundle)
        {
            CrashManager.Register (this, "da7c75f8b96b4b3abc8569120d7e1b3a");

            Xamarin.Forms.Forms.Init (this, bundle);
            Xamarin.FormsMaps.Init (this, bundle);
			global::Xamarin.Forms.Forms.SetTitleBarVisibility(Xamarin.Forms.AndroidTitleBarVisibility.Never);

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => {
                        return true;
                    };

            base.OnCreate (bundle);

            var mvxFormsApp = new FormsApp ();
            LoadApplication (mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter> () as MvxFormsDroidMasterDetailPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart> ().Start ();


            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            //App.HardwareBackPressed = () =>
            //{
            //	MoveTaskToBack(true);
            //};
        }

        protected override void OnStart ()
        {
            base.OnStart ();
            IsForeground = true;
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            IsForeground = true;
        }

        protected override void OnStop ()
        {
            base.OnStop ();
            IsForeground = false;
        }

        protected override void OnPause ()
        {
            base.OnPause ();
            IsForeground = false;
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			if (requestCode == RequestReadContacts)
			{
				var contactService = Mvx.Resolve<IContactService>() as ContactsService;
				contactService.OnRequestPermissionsResult(grantResults[0] == Permission.Granted);
			}
		}
    }
}

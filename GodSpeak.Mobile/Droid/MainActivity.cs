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

namespace GodSpeak.Droid
{
    [Activity(Theme = "@style/AppTheme", Label = "MvxFormsApplicationActivity", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
		{
			Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);

			ServicePointManager
				.ServerCertificateValidationCallback +=
					(sender, cert, chain, sslPolicyErrors) =>
					{
						return true;
					};

			base.OnCreate(bundle);

			var mvxFormsApp = new MvxFormsApp();
			LoadApplication(mvxFormsApp);

			var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidMasterDetailPagePresenter;
			presenter.MvxFormsApp = mvxFormsApp;

			Mvx.Resolve<IMvxAppStart>().Start();

			//App.HardwareBackPressed = () =>
			//{
			//	MoveTaskToBack(true);
			//};
		}
    }
}

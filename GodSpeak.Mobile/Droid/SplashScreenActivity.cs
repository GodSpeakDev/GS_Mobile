using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using MvvmCross.Droid.Views;
using Xamarin.Forms;

namespace GodSpeak.Droid
{
    [Activity (
        Label = "GodSpeak"
        , MainLauncher = true
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity ()
            : base (Resource.Layout.SplashScreen)
        {
        }

        protected override void OnCreate (Bundle bundle)
        {
            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) => {
                if (!string.IsNullOrWhiteSpace (e.View.StyleId)) {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            base.OnCreate (bundle);
        }

        private bool isInitializationComplete = false;
        public override void InitializationComplete ()
        {
            if (!isInitializationComplete) {
                isInitializationComplete = true;
                StartActivity (typeof (MainActivity));
            }
        }
    }
}

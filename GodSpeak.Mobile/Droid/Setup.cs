using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using GodSpeak;
using Xamarin.Forms;
using GodSpeak.Services;
using GodSpeak.Droid.Services;
using MvvmCross.Plugins.WebBrowser;
using MvvmCross.Forms.Droid;

namespace GodSpeak.Droid
{
    public class Setup : MvxFormsAndroidSetup
    {
        public Setup (Context applicationContext) : base (applicationContext)
        {
        }

        protected override IMvxApplication CreateApp ()
        {
            Mvx.LazyConstructAndRegisterSingleton<IFeedbackService, FeedbackService> ();
            Mvx.LazyConstructAndRegisterSingleton<IMediaPicker, MediaPicker> ();
            Mvx.LazyConstructAndRegisterSingleton<IShareService, ShareService> ();
            Mvx.LazyConstructAndRegisterSingleton<IReminderService, ReminderService> ();
            Mvx.LazyConstructAndRegisterSingleton<IProgressHudService, ProgressHudService> ();
			Mvx.LazyConstructAndRegisterSingleton<IContactService, ContactsService>();
			Mvx.LazyConstructAndRegisterSingleton<IMailService, MailService>();
			Mvx.LazyConstructAndRegisterSingleton<IImageService, ImageService>();
			Mvx.LazyConstructAndRegisterSingleton<IFileService, FileService>();
			Mvx.LazyConstructAndRegisterSingleton<ILogManager, NLogManager>();
			Mvx.LazyConstructAndRegisterSingleton<ILoggingService, LoggingService>();
			Mvx.LazyConstructAndRegisterSingleton<IMvxWebBrowserTask, CustomDroidWebBrowserTask>();

            return new App ();
        }

        protected override IMvxTrace CreateDebugTrace ()
        {
            return new DebugTrace ();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter ()
        {
            var presenter = new MvxFormsDroidPagePresenterCustom ();
            Mvx.RegisterSingleton<IMvxViewPresenter> (presenter);

            return presenter;
        }

        private class MvxFormsDroidPagePresenterCustom : MvxFormsDroidMasterDetailPagePresenter
        {
            public MvxFormsDroidPagePresenterCustom ()
            {
            }

            public MvxFormsDroidPagePresenterCustom (Xamarin.Forms.Application mvxFormsApp) : base(mvxFormsApp)
            {
            }
        }
    }
}

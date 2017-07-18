using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using UIKit;
using GodSpeak;
using Xamarin.Forms;
using GodSpeak.Services;
using GodSpeak.iOS.Services;

namespace GodSpeak.iOS
{
    public class Setup : MvvmCross.Forms.iOS.MvxFormsIosSetup
    {
        public Setup (MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base (applicationDelegate, window)
        {
        }

        //public Setup (MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        //    : base (applicationDelegate, presenter)
        //{
        //}

        protected override IMvxIosViewPresenter CreatePresenter ()
        {
            Forms.Init ();
            Xamarin.FormsMaps.Init ();

            var xamarinFormsApp = new FormsApp ();

            return new MvxFormsIosPagePresenterCustom (Window, xamarinFormsApp);
        }

        protected override IMvxApplication CreateApp ()
        {
            Mvx.LazyConstructAndRegisterSingleton<IMediaPicker, MediaPicker> ();
            Mvx.LazyConstructAndRegisterSingleton<IShareService, ShareService> ();
            Mvx.LazyConstructAndRegisterSingleton<IReminderService, ReminderService> ();
            Mvx.LazyConstructAndRegisterSingleton<IContactService, ContactsService> ();
            Mvx.LazyConstructAndRegisterSingleton<IMailService, MailService> ();
            Mvx.LazyConstructAndRegisterSingleton<IFeedbackService, FeedbackService> ();
            Mvx.LazyConstructAndRegisterSingleton<IProgressHudService, ProgressHudService> ();
			Mvx.LazyConstructAndRegisterSingleton<IImageService, ImageService>();
			Mvx.LazyConstructAndRegisterSingleton<IFileService, FileService>();
			Mvx.LazyConstructAndRegisterSingleton<ILoggingService, LoggingService>();
			Mvx.LazyConstructAndRegisterSingleton<ILogManager, NLogManager>();

            return new App ();
        }

        protected override IMvxTrace CreateDebugTrace ()
        {			
			return new DebugTrace();
        }

        private class MvxFormsIosPagePresenterCustom : MvxFormsIosMasterDetailPagePresenter
        {
            public MvxFormsIosPagePresenterCustom (UIWindow window, Xamarin.Forms.Application application)
                : base (window, application)
            {
            }
        }
    }
}

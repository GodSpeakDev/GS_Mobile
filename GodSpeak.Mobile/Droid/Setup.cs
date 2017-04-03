using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Presenter.Droid;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using GodSpeak;
using Xamarin.Forms;
using MvvmCross.Forms.Presenter.Core;
using GodSpeak.Services;
using GodSpeak.Droid.Services;

namespace GodSpeak.Droid
{
    public class Setup : MvxAndroidSetup
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

            public MvxFormsDroidPagePresenterCustom (MvxFormsApp mvxFormsApp) : base (mvxFormsApp)
            {
            }
        }
    }
}

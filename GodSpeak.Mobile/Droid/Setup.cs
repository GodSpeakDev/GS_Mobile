using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Presenter.Droid;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using GodSpeak;

namespace GodSpeak.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			Mvx.LazyConstructAndRegisterSingleton<IMediaPicker, MediaPicker>();

            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var presenter = new MvxFormsDroidPagePresenterCustom();
			Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

			return presenter;
		}

		private class MvxFormsDroidPagePresenterCustom : MvxFormsDroidMasterDetailPagePresenter
		{
			
		}
    }
}

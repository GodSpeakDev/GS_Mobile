using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;
using GodSpeak;
using Xamarin.Forms;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Forms.Presenter.iOS;

namespace GodSpeak.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }
        
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

		protected override IMvxIosViewPresenter CreatePresenter()
		{
			Forms.Init();

			var xamarinFormsApp = new MvxFormsApp();

			return new MvxFormsIosPagePresenterCustom(Window, xamarinFormsApp);
		}

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
        
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		private class MvxFormsIosPagePresenterCustom : MvxFormsIosMasterDetailPagePresenter
		{
			public MvxFormsIosPagePresenterCustom(UIWindow window, Xamarin.Forms.Application application)
				: base(window, application)
			{
			}
		}
    }
}

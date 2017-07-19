using MvvmCross.iOS.Views.Presenters;
using UIKit;
using Xamarin.Forms;
using MvvmCross.Core.ViewModels;
using System;

namespace GodSpeak.iOS
{
	public class MvxFormsIosMasterDetailPagePresenter
		: MvxFormsMasterDetailPagePresenter
		, IMvxIosViewPresenter
	{
		private readonly UIWindow _window;

		public MvxFormsIosMasterDetailPagePresenter(UIWindow window, Xamarin.Forms.Application mvxFormsApp)
			: base(mvxFormsApp)
		{
			_window = window;
		}

		public virtual bool PresentModalViewController(UIViewController controller, bool animated)
		{
			return false;
		}

		public virtual void NativeModalViewControllerDisappearedOnItsOwn()
		{
		}

		protected override void CustomPlatformInitialization(Page mainPage)
		{
			_window.RootViewController = mainPage.CreateViewController();
		}

        public override void Close(IMvxViewModel toClose)
        {
            
        }
    }
}
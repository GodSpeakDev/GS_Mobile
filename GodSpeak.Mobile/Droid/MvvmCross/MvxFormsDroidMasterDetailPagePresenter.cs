using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace GodSpeak.Droid
{
	public class MvxFormsDroidMasterDetailPagePresenter
		: MvxFormsMasterDetailPagePresenter
		, IMvxAndroidViewPresenter
	{
		public MvxFormsDroidMasterDetailPagePresenter()
		{
		}

		public MvxFormsDroidMasterDetailPagePresenter(Xamarin.Forms.Application mvxFormsApp)
			: base(mvxFormsApp)
		{
		}

        public override void Close(IMvxViewModel toClose)
        {
            throw new NotImplementedException();
        }
    }
}
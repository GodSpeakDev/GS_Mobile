using MvvmCross.Droid.Views;
using MvvmCross.Forms.Presenter.Core;

namespace GodSpeak.Droid
{
	public class MvxFormsDroidMasterDetailPagePresenter
		: MvxFormsMasterDetailPagePresenter
		, IMvxAndroidViewPresenter
	{
		public MvxFormsDroidMasterDetailPagePresenter()
		{
		}

		public MvxFormsDroidMasterDetailPagePresenter(MvxFormsApp mvxFormsApp)
			: base(mvxFormsApp)
		{
		}
	}
}
using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class GetStartedViewModel : CustomViewModel
	{
		private WelcomeViewModel _parentViewModel;

		private MvxCommand tapGetStartedCommand;
		public MvxCommand TapGetStartedCommand
		{
			get
			{
				return tapGetStartedCommand ?? (tapGetStartedCommand = new MvxCommand(DoTapGetStartedCommand));
			}
		}

		public GetStartedViewModel(WelcomeViewModel parentViewModel, IDialogService dialogService) : base(dialogService)
		{
			_parentViewModel = parentViewModel;
		}

		private void DoTapGetStartedCommand()
		{
			_parentViewModel.SelectPage<ClaimInviteCodeViewModel>();
		}
	}
}

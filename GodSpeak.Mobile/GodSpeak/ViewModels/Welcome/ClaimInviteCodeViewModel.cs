using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class ClaimInviteCodeViewModel : CustomViewModel
	{
		private WelcomeViewModel _parentViewModel;

		private string _inviteCode;
		public string InviteCode
		{
			get { return _inviteCode;}
			set { SetProperty(ref _inviteCode, value);}
		}

		private MvxCommand _claimInviteCodeCommand;
		public MvxCommand ClaimInviteCodeCommand
		{
			get
			{
				return _claimInviteCodeCommand ?? (_claimInviteCodeCommand = new MvxCommand(DoClaimInviteCodeCommand));
			}
		}

		private MvxCommand _dontHaveCodeCommand;
		public MvxCommand DontHaveCodeCommand
		{
			get
			{
				return _dontHaveCodeCommand ?? (_dontHaveCodeCommand = new MvxCommand(DoDontHaveCodeCommand));
			}
		}

		private MvxCommand _alreadyRegisteredCommand;
		public MvxCommand AlreadyRegisteredCommand
		{
			get
			{
				return _alreadyRegisteredCommand ?? (_alreadyRegisteredCommand = new MvxCommand(DoAlreadyRegisteredCommand));
			}
		}

		public ClaimInviteCodeViewModel(WelcomeViewModel parentViewModel)
		{
			_parentViewModel = parentViewModel;
		}

		public void DoClaimInviteCodeCommand()
		{
			
		}

		public void DoDontHaveCodeCommand()
		{
			_parentViewModel.SelectPage<RequestInviteCodeViewModel>();
		}

		public void DoAlreadyRegisteredCommand()
		{
			ShowViewModel<LoginViewModel>();
		}
	}
}

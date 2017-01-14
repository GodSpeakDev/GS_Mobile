using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class ForgotPasswordViewModel : CustomViewModel
	{
		private string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private MvxCommand _sendInstructionsCommand;
		public MvxCommand SendInstructionsCommand
		{
			get
			{
				return _sendInstructionsCommand ?? (_sendInstructionsCommand = new MvxCommand(DoSendInstructionsCommand));
			}
		}

		public ForgotPasswordViewModel()
		{
		}

		private void DoSendInstructionsCommand()
		{
			
		}
	}
}

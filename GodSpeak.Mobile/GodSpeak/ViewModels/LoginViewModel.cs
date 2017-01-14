using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class LoginViewModel : CustomViewModel
	{
		private string _email;
		public string Email
		{
			get { return _email;}
			set { SetProperty(ref _email, value);}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set { SetProperty(ref _password, value); }
		}

		private MvxCommand _loginCommand;
		public MvxCommand LoginCommand
		{
			get
			{
				return _loginCommand ?? (_loginCommand = new MvxCommand(DoLoginCommand));
			}
		}

		private MvxCommand _registerCommand;
		public MvxCommand RegisterCommand
		{
			get
			{
				return _registerCommand ?? (_registerCommand = new MvxCommand(DoRegisterCommand));
			}
		}

		private MvxCommand _forgotPasswordCommand;
		public MvxCommand ForgotPasswordCommand
		{
			get
			{
				return _forgotPasswordCommand ?? (_forgotPasswordCommand = new MvxCommand(DoForgotPasswordCommand));
			}
		}

		public LoginViewModel()
		{
		}

		private void DoLoginCommand()
		{
			this.ShowViewModel<HomeViewModel>();
		}

		private void DoRegisterCommand()
		{
			this.ShowViewModel<RegisterViewModel>();
		}

		private void DoForgotPasswordCommand()
		{
			this.ShowViewModel<ForgotPasswordViewModel>();
		}
	}
}

using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class LoginViewModel : CustomViewModel
	{
		private MvxCommand tapLoginCommand;
		public MvxCommand TapLoginCommand
		{
			get
			{
				return tapLoginCommand ?? (tapLoginCommand = new MvxCommand(DoTapLoginCommand));
			}
		}

		public LoginViewModel()
		{
		}

		private void DoTapLoginCommand()
		{
			this.ShowViewModel<HomeViewModel>();
		}
	}
}

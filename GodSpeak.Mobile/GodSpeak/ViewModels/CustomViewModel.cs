using System;
using MvvmCross.Core.ViewModels;


namespace GodSpeak
{
	public class CustomViewModel : MvxViewModel
	{
		private MvxCommand closeCommand;
		public MvxCommand CloseCommand
		{
			get { return closeCommand ?? (closeCommand = new MvxCommand(DoCloseCommand)); }
		}

		public CustomViewModel()
		{
		}

		protected void DoCloseCommand()
		{
			this.Close(this);
		}
	}
}

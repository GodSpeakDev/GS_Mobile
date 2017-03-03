using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class GetStartedViewModel : CustomViewModel
	{
		public Action ToggleViewVisibility
		{
			get;
			set;
		}

		public GetStartedViewModel(IDialogService dialogService) : base(dialogService)
		{
			
		}

		private MvxCommand tapGetStartedCommand;
		public MvxCommand TapGetStartedCommand
		{
			get
			{
				return tapGetStartedCommand ?? (tapGetStartedCommand = new MvxCommand(DoTapGetStartedCommand));
			}
		}

		private void DoTapGetStartedCommand()
		{
			if (ToggleViewVisibility != null)
			{
				ToggleViewVisibility();
			}
		}
	}
}

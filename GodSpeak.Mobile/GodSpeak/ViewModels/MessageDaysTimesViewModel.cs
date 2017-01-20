using MvvmCross.Core.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Forms.Presenter.Core;
using System.Windows.Input;

namespace GodSpeak
{
	public class MessageDaysTimesViewModel : CustomViewModel
	{
		public MessageDaysTimesViewModel(IDialogService dialogService) : base(dialogService)
		{
		}
	}
}

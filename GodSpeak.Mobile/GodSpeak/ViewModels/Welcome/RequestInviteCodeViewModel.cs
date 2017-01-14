using System;
using MvvmCross.Core.ViewModels;

namespace GodSpeak
{
	public class RequestInviteCodeViewModel : CustomViewModel
	{
		private string _email;
		public string Email
		{
			get { return _email; }
			set { SetProperty(ref _email, value); }
		}

		private MvxCommand _requestInviteCodeCommand;
		public MvxCommand RequestInviteCodeCommand
		{
			get
			{
				return _requestInviteCodeCommand ?? (_requestInviteCodeCommand = new MvxCommand(DoRequestInviteCodeCommand));
			}
		}

		private MvxCommand _purchaseCreditCommand;
		public MvxCommand PurchaseCreditCommand
		{
			get
			{
				return _purchaseCreditCommand ?? (_purchaseCreditCommand = new MvxCommand(DoPurchaseCreditCommand));
			}
		}

		public RequestInviteCodeViewModel()
		{
		}

		public void DoRequestInviteCodeCommand()
		{

		}

		public void DoPurchaseCreditCommand()
		{

		}
	}
}

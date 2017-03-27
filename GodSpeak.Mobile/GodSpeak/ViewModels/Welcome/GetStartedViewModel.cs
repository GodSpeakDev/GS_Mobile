using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class GetStartedViewModel : CustomViewModel
	{
		public Action ShowInviteCodeBox
		{
			get;
			set;
		}

		public Action ShowGiftCodeSuccessBox
		{
			get;
			set;
		}

		private string _giftCodeText;
		public string GiftCodeText
		{
			get { return _giftCodeText; }
			set { 
				SetProperty(ref _giftCodeText, value);
				RaisePropertyChanged(nameof(IsSubmitInvideCodeValid));
			}
		}

		public bool IsSubmitInvideCodeValid
		{
			get 
			{
				var isValid = !string.IsNullOrEmpty(GiftCodeText);
				return isValid;
			}
		}

		public GetStartedViewModel(IDialogService dialogService) : base(dialogService)
		{
			
		}

		public void Init()
		{
			GiftCodeText = string.Empty;
		}

		private MvxCommand _tapGetStartedCommand;
		public MvxCommand TapGetStartedCommand
		{
			get
			{
				return _tapGetStartedCommand ?? (_tapGetStartedCommand = new MvxCommand(DoTapGetStartedCommand));
			}
		}

		private MvxCommand _submitGiftCodeCommand;
		public MvxCommand SubmitGiftCodeCommand
		{
			get
			{
				return _submitGiftCodeCommand ?? (_submitGiftCodeCommand = new MvxCommand(DoSubmitGiftCodeCommand));
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

		private MvxCommand _registerCommand;
		public MvxCommand RegisterCommand
		{
			get
			{
				return _registerCommand ?? (_registerCommand = new MvxCommand(DoRegisterCommand));
			}
		}

		private void DoTapGetStartedCommand()
		{
			if (ShowInviteCodeBox != null)
			{
				ShowInviteCodeBox();
			}
		}

		private void DoSubmitGiftCodeCommand()
		{
			if (ShowGiftCodeSuccessBox != null)
			{
				ShowGiftCodeSuccessBox();
			}
		}

		private void DoRegisterCommand()
		{
			this.ShowViewModel<RegisterViewModel>();
		}

		private async void DoDontHaveCodeCommand()
		{
			var result = await this.DialogService.ShowInputPopup(Text.AnonymousTitle, Text.AnonymousText, new InputOptions() {Placeholder=Text.AnonymousInputPlaceholder}, Text.AnonymousSubmit, Text.AnonymousNevermind);

			if (result.SelectedButton == Text.AnonymousSubmit)
			{
				await this.DialogService.ShowAlert(Text.AnonymousSuccessTitle, string.Format(Text.AnonymousSuccessText, result.InputText), Text.AnonymousSuccessButtonTitle);
			}
		}

		private void DoAlreadyRegisteredCommand()
		{
			this.ShowViewModel<LoginViewModel>();
		}
	}
}

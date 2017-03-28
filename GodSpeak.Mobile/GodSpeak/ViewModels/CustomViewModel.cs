using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class CustomViewModel : MvxViewModel
	{
		public IDialogService DialogService { get; private set;}

		private MvxCommand closeCommand;
		public MvxCommand CloseCommand
		{
			get { return closeCommand ?? (closeCommand = new MvxCommand(DoCloseCommand)); }
		}

		public CustomViewModel(IDialogService dialogService)
		{
			DialogService = dialogService;
		}

		protected void DoCloseCommand()
		{
			this.Close(this);
		}

		public async Task HandleResponse<T>(ApiResponse<T> response)
		{
			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				//Settings.Token = null;
				//await this.ShowAlert(Text.SessionExpiredPopupTitle, Text.SessionExpiredPopupTitle);
				//await GoToRoot();
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
			{
				//await this.ShowAlert(Text.NoConnectionPopupTitle, Text.NoConnectionPopupText);
			}
			else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			{
				await DialogService.ShowAlert(response.Title ?? Text.ErrorPopupTitle, 
				                              response.Message ?? Text.ErrorPopupText);
			}
			else
			{
				
				//await this.ShowAlert(Text.ErrorPopupTitle, Text.ErrorPopupText);
			}
		}
	}
}

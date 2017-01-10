using System;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

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

		public async Task HandleResponse<T>(BaseResponse<T> response)
		{
			//if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			//{
			//	Settings.Token = null;
			//	await this.ShowAlert(Text.SessionExpiredPopupTitle, Text.SessionExpiredPopupTitle);
			//	await GoToRoot();
			//}
			//else if (response.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
			//{
			//	await this.ShowAlert(Text.NoConnectionPopupTitle, Text.NoConnectionPopupText);
			//}
			//else
			//{
			//	await this.ShowAlert(Text.ErrorPopupTitle, Text.ErrorPopupText);
			//}
		}
	}
}

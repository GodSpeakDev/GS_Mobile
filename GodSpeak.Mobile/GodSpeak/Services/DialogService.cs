using System;
using System.Threading.Tasks;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class DialogService : IDialogService
	{
		public Func<string, string, string, Task> DoShowAlert { get; set; }
		public Func<string, string, string, string, Task<bool>> DoShowConfirmation { get; set; }

		public async Task ShowAlert(string title, string message)
		{
			if (DoShowAlert != null)
			{
				await DoShowAlert(title, message, Text.OkPopup);
			}
		}

		public async Task<bool> ShowConfirmation(string title, string message, string acceptText, string cancelText)
		{
			if (DoShowConfirmation != null)
			{
				return await DoShowConfirmation(title, message, acceptText, cancelText);
			}

			return false;
		}
	}
}

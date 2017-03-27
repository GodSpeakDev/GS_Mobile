using System;
using System.Threading.Tasks;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class DialogService : IDialogService
	{
		public Func<string, string, string, Task> DoShowAlert { get; set; }
		public Func<string, string, string, string, Task<bool>> DoShowConfirmation { get; set; }
		public Func<string, string, string[], Task<string>> DoShowMenu { get; set; }
		public Func<string, string, InputOptions, string[], Task<InputResult>> DoShowInputPopup { get; set; }

		public async Task ShowAlert(string title, string message, string buttonText = null)
		{
			buttonText = buttonText ?? Text.OkPopup;
			if (DoShowAlert != null)
			{
				await DoShowAlert(title, message, buttonText);
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

		public async Task<string> ShowMenu(string title, string message, params string[] buttons)
		{
			if (DoShowMenu != null)
			{
				return await DoShowMenu(title, message, buttons);
			}

			return null;
		}

		public async Task<InputResult> ShowInputPopup(string title, string message, InputOptions inputOptions, params string[] buttons)
		{
			if (DoShowInputPopup != null)
			{
				return await DoShowInputPopup(title, message, inputOptions, buttons);
			}

			return null;
		}
	}
}

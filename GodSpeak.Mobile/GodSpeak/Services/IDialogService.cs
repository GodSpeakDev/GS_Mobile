using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IDialogService
	{
		Task ShowAlert(string title, string message, string buttonText = null);
		Task<bool> ShowConfirmation(string title, string message, string acceptText, string cancelText);
		Task<string> ShowMenu(string title, string message, params string[] buttons);
		Task<InputResult> ShowInputPopup(string title, string message, InputOptions inputOptions, params string[] buttons);
	}
}

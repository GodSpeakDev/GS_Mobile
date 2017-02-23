using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IDialogService
	{
		Task ShowAlert(string title, string message);
		Task<bool> ShowConfirmation(string title, string message, string acceptText, string cancelText);
		Task<string> ShowMenu(string title, string message, params string[] buttons);
	}
}

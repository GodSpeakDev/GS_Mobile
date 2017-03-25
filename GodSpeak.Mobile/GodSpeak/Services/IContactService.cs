using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GodSpeak
{
	public interface IContactService
	{
		void LaunchContactBrowser(Action<Contact> onSelectedAction);
		Task<Contact> ParseContact(object contact);
		void GetAllContacts(Action<List<Contact>> resultAction);
		Task<bool> CanAccessContacts();
		void DisplayAccessDenied();
	}
}

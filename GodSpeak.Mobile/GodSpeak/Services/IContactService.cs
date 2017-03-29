using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GodSpeak
{
	public interface IContactService
	{		
		void GetAllContacts(Action<List<Contact>> resultAction);
		Task<bool> CanAccessContacts();
	}
}

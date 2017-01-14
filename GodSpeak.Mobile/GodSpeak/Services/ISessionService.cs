using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface ISessionService
	{
		Task SaveUser(User user);
		Task ClearUser();
	}
}

using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class SessionService : ISessionService
	{
		private User _user;

		public async Task SaveUser(User user)
		{
			await Task.Delay(1);
			_user = user;
		}

		public async Task ClearUser()
		{
			await Task.Delay(1);
			_user = null;
		}
	}
}

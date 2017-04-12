using System;
using System.Threading.Tasks;

namespace GodSpeak
{
	public class SessionService : ISessionService
	{
		private User _user;
		private IWebApiService _webApiService;
		private ISettingsService _settingsService;

		public SessionService(IWebApiService webApiService, ISettingsService settingsService)
		{
			_webApiService = webApiService;
			_settingsService = settingsService;
		}

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

		public async Task<User> GetUser()
		{
			if (_user == null)
			{
				var response = await _webApiService.GetProfile(new TokenRequest {Token=_settingsService.Token});
				_user = response.Payload;
			}

			return _user;
		}
	}
}

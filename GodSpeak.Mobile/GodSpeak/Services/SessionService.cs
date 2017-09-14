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

		private static Task<User> _getUserTask;
		public Task<User> GetUser()
		{
			if (string.IsNullOrEmpty(_settingsService.Token))
				return null;
			
			if (_getUserTask == null)
			{
				_getUserTask = MakeUserCall();
			}

			if (_getUserTask != null && _getUserTask.IsCompleted)
			{
				if (_getUserTask.Result == null)
				{
					_getUserTask = MakeUserCall();
				}
			}

			return _getUserTask;
		}

		private async Task<User> MakeUserCall() 
		{			
			var result = await _webApiService.GetProfile();
			if (result.IsSuccess)
			{
				return result.Payload;
			}
			else if (result.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
			{
				_getUserTask = null;
				return new User()
				{
					Token = _settingsService.Token,
					Email = _settingsService.Email
				};
			}
			else
			{
				return null;
			}
		}
	}
}

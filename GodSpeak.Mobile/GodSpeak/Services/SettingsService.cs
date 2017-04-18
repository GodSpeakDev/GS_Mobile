using System;
using System.Collections.Generic;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using System.Linq;

namespace GodSpeak
{
	public class SettingsService : ISettingsService
	{
		private ISettings _settings;

		private string _verseCodesKey = "Godspeak.VerseCodes";
		private string _verseCodesDefault = string.Empty;

		private string _tokenKey = "Godspeak.Token";
		private string _tokenDefault = null;

		private string _remindersKey = "Godspeak.Reminders";
		private string _remindersDefault = string.Empty;

		private string _emailKey = "Godspeak.Email";
		private string _emailDefault = null;

		public SettingsService(ISettings settings)
		{
			_settings = settings;
		}

		public List<string> DeliveredVerseCodes
		{
			get 
			{ 
				var str = _settings.GetValue <string>(_verseCodesKey, _verseCodesDefault);
				return new List<string>(str.Split(','));
			}
			set 
			{ 
				_settings.AddOrUpdateValue<string>(_verseCodesKey, string.Join(",", value));
			}
		}

		public List<int> ReminderIds
		{
			get
			{
				var str = _settings.GetValue<string>(_remindersKey, _remindersDefault);
				if (string.IsNullOrEmpty(str))
				{
					return new List<int>();	
				}

				return new List<int>(new List<string>(str.Split(',')).Select(x => int.Parse(x)));
			}
			set
			{
				_settings.AddOrUpdateValue<string>(_remindersKey, string.Join(",", value));
			}
		}

		public string Token
		{
			get 
			{
				return _settings.GetValue<string>(_tokenKey, _tokenDefault);
			}
			set 
			{
				if (string.IsNullOrEmpty(value) && _settings.Contains(_tokenKey))
				{
					_settings.DeleteValue(_tokenKey);
				}
				else
				{
					_settings.AddOrUpdateValue<string>(_tokenKey, value);
				}
			}
		}

		public string Email
		{
			get
			{
				return _settings.GetValue<string>(_emailKey, _emailDefault);
			}
			set
			{
				if (string.IsNullOrEmpty(value) && _settings.Contains(_emailKey))
				{
					_settings.DeleteValue(_emailKey);
				}
				else
				{
					_settings.AddOrUpdateValue<string>(_emailKey, value);
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using Cheesebaron.MvxPlugins.Settings.Interfaces;

namespace GodSpeak
{
	public class SettingsService : ISettingsService
	{
		private ISettings _settings;

		private string _verseCodesKey = "Godspeak.VerseCodes";
		private string _verseCodesDefault = string.Empty;

		private string _tokenKey = "Godspeak.Token";
		private string _tokenDefault = null;

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
	}
}

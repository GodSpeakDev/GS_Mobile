using System;
using System.Text.RegularExpressions;

namespace GodSpeak
{
	public static class StringExtensions
	{
		public static bool IsValidPassword(this string password)
		{
			var numberDetector = new Regex(@"\d{1}?");
			var lowerCaseDetector = new Regex(@"[a-z]{1}?");
			var upperCaseDetector = new Regex(@"[A-Z]{1}?");

			return string.IsNullOrEmpty(password) || (password.Length >= 6 && numberDetector.IsMatch(password) && lowerCaseDetector.IsMatch(password) && upperCaseDetector.IsMatch(password));
		}
	}
}

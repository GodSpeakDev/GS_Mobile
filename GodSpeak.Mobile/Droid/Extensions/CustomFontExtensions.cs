using System;

namespace GodSpeak.Droid
{
	public static class CustomFontExtensions
	{
		public static Android.Graphics.TypefaceStyle GetFont(this ICustomFont customElement)
		{
			switch (customElement.FontWeight)
			{
				case GodSpeak.FontWeight.Light:
					return Android.Graphics.TypefaceStyle.Normal;
				case GodSpeak.FontWeight.Regular:
					return Android.Graphics.TypefaceStyle.Normal;
				case GodSpeak.FontWeight.Medium:
					return Android.Graphics.TypefaceStyle.Normal;
				case GodSpeak.FontWeight.Semibold:
					return Android.Graphics.TypefaceStyle.Bold;
				case GodSpeak.FontWeight.Bold:
					return Android.Graphics.TypefaceStyle.Bold;
				case GodSpeak.FontWeight.Heavy:
					return Android.Graphics.TypefaceStyle.Bold;
			}

			return Android.Graphics.TypefaceStyle.Normal;
		}
	}
}

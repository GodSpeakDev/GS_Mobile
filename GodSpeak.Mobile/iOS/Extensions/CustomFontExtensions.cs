using System;
using UIKit;

namespace GodSpeak.iOS
{
	public static class CustomFontExtensions
	{
		public static UIFont GetUIFont(this ICustomFont customElement)
		{
			if (customElement == null)
				return null;

			switch (customElement.FontWeight)
			{
				case GodSpeak.FontWeight.Light:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Light);					
				case GodSpeak.FontWeight.Regular:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Regular);					
				case GodSpeak.FontWeight.Medium:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Medium);					
				case GodSpeak.FontWeight.Semibold:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Semibold);					
				case GodSpeak.FontWeight.Bold:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Bold);					
				case GodSpeak.FontWeight.Heavy:
					return UIFont.SystemFontOfSize((nfloat)customElement.FontSize, UIFontWeight.Heavy);
			}

			return null;
		}
	}
}

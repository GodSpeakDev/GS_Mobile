using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class ImpactFontConverter
	{
		public object GetFontSizeForCount(int total)
		{
			total = 2555;
			var isIPhone5 = Device.RuntimePlatform == Device.iOS && App.ScreenWidth == 320;
			var isIos = Device.RuntimePlatform == Device.iOS;

			if (total >= 1000)
			{
				if (isIPhone5)
				{
					return 15;
				}

				return 20;
			}
			else
			{
				if (isIPhone5)
				{
					return 25;
				}

				return 35;
			}
		}
	}
}

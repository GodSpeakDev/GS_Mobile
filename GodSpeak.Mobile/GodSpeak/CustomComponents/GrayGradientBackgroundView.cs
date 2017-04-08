using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class GrayGradientBackgroundView : GradientBoxView
	{
		public GrayGradientBackgroundView()
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				Colors = new Color[]
				{
					ColorHelper.IosDarkGrayGradient,
					ColorHelper.IosLightGrayGradient,
				};
			}
			else
			{
				Colors = new Color[]
				{
					ColorHelper.DarkGray,
					ColorHelper.LightGray,
				};
			}

			BackgroundColor = Color.Transparent;
		}
	}
}

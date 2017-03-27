using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class GrayGradientBackgroundView : GradientBoxView
	{
		public GrayGradientBackgroundView()
		{
			Colors = new Color[] 
			{
				ColorHelper.IosDarkGrayGradient,
				ColorHelper.IosLightGrayGradient,
			};
			BackgroundColor = Color.Transparent;
		}
	}
}

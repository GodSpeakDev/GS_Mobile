using System;
using Xamarin.Forms;
namespace GodSpeak
{
	public class MessageGradientView : GradientBoxView
	{
		public MessageGradientView()
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
					Color.FromRgb(92, 94, 96)
				};
			}

			BackgroundColor = Color.Transparent;
		}
	}
}

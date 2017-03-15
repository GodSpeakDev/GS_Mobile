using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class MessageDetailGradientView : GradientBoxView
	{
		public MessageDetailGradientView()
		{
			Colors = new Color[]
			{
				ColorHelper.IosDarkBlueGradient,
				ColorHelper.IosLightBlueGradient,
			};
			BackgroundColor = Color.Transparent;
		}
	}
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GodSpeak;
using GodSpeak.iOS;
using UIKit;

[assembly: ExportRendererAttribute(typeof(CustomContentPage), typeof(CustomContentPageRenderer))]
namespace GodSpeak.iOS
{
	public class CustomContentPageRenderer : PageRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
		}
	}
}

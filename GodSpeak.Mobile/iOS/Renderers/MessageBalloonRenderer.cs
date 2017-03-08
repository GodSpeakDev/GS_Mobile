using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(MessageBalloon), typeof(MessageBalloonRenderer))]
namespace GodSpeak.iOS
{
	public class MessageBalloonRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (this.Control != null)
			{				
				//this.Control.Layer.MasksToBounds = true;
				//this.Control.Layer.CornerRadius = 5.0f;
				//this.Control.Layer.BackgroundColor = ColorHelper.Primary.ToCGColor();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			//this.Layer.MasksToBounds = true;
			//this.Layer.CornerRadius = 5.0f;
			//this.Layer.BackgroundColor = ColorHelper.Primary.ToCGColor();
		}
	}
}

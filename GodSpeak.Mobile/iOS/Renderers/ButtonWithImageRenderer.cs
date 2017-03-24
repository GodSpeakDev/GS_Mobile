using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(ButtonWithImage), typeof(ButtonWithImageRenderer))]
namespace GodSpeak.iOS
{
	public class ButtonWithImageRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (this.Control != null)
			{
				this.Control.Layer.MasksToBounds = true;
				this.Control.Layer.CornerRadius = 5.0f;
				this.Control.Layer.BackgroundColor = ColorHelper.Primary.ToCGColor();
				this.Control.Layer.BorderWidth = 0;
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.Layer.BorderWidth = 2;
			this.Layer.MasksToBounds = true;
			this.Layer.CornerRadius = 5.0f;
			this.Layer.BackgroundColor = ColorHelper.Primary.ToCGColor();
			this.Layer.BorderWidth = 0;
		}
	}
}

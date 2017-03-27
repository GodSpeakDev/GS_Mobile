using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GodSpeak;
using GodSpeak.iOS;
using UIKit;

[assembly: ExportRendererAttribute(typeof(PlusMinusButtonView), typeof(PlusMinusButtonViewRenderer))]
namespace GodSpeak.iOS
{
	public class PlusMinusButtonViewRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (this.Control != null)
			{
				this.Control.Layer.BorderWidth = 2;
				this.Control.Layer.MasksToBounds = true;
				this.Control.Layer.CornerRadius = 5.0f;
				this.Control.Layer.BorderColor = ColorHelper.Secondary.ToCGColor();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.Layer.BorderWidth = 2;
			this.Layer.MasksToBounds = true;
			this.Layer.CornerRadius = 5.0f;
			this.Layer.BorderColor = ColorHelper.Secondary.ToCGColor();
		}
	}
}

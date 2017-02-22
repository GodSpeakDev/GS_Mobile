using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(ImageContainer), typeof(ImageContainerRenderer))]
namespace GodSpeak.iOS
{
	public class ImageContainerRenderer : ViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (this.Control != null)
			{
				this.Control.Layer.BorderWidth = 1;
				this.Control.Layer.MasksToBounds = true;
				this.Control.Layer.CornerRadius = 5.0f;
				this.Control.Layer.BorderColor = ColorHelper.OutlinePlaceHolder.ToCGColor();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.Layer.BorderWidth = 1;
			this.Layer.MasksToBounds = true;
			this.Layer.CornerRadius = 5.0f;
			this.Layer.BorderColor = ColorHelper.OutlinePlaceHolder.ToCGColor();
		}
	}
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace GodSpeak.iOS
{
	public class CustomButtonRenderer : ButtonRenderer
	{
		public CustomButton CustomButton
		{
			get { return Element as CustomButton; }
		}

		public CustomButtonRenderer()
		{
			
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
            SetTextAlignment();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetTextAlignment();
		}

		private void SetTextAlignment()
		{
			if (Element == null)
				return;

			if (CustomButton.TextAlignment == TextAlignment.Start)
			{
				this.Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
				this.Control.ContentEdgeInsets = new UIEdgeInsets(0, 10, 0, 0);
			}
			else if (CustomButton.TextAlignment == TextAlignment.Center)
			{
				this.Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                this.Control.ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
			}
			else if (CustomButton.TextAlignment == TextAlignment.Center)
			{
                this.Control.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                this.Control.ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 10);
			}
		}
	}
}

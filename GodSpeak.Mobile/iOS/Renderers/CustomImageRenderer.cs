using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using GodSpeak;
using GodSpeak.iOS;
using Xamarin.Forms;
using System.ComponentModel;
using CoreGraphics;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace GodSpeak.iOS
{
	public class CustomImageRenderer : ImageRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
				return;

			CreateLayer();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == VisualElement.HeightProperty.PropertyName || e.PropertyName == VisualElement.WidthProperty.PropertyName)
			{
				CreateLayer();
			}
		}

		private void CreateLayer()
		{
			try
			{
				var element = (CustomImage)Element;

				Control.Layer.CornerRadius = (float)element.BorderRadius;
				Control.Layer.MasksToBounds = false;
				Control.Layer.BorderColor = element.BorderColor.ToCGColor();
				Control.Layer.BorderWidth = (float)element.BorderSize;
				Control.ClipsToBounds = true;
			}
			catch { }
		}
	}
}

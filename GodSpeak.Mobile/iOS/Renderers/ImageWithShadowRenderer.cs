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

[assembly: ExportRenderer(typeof(ImageWithShadow), typeof(ImageWithShadowRenderer))]
namespace GodSpeak.iOS
{
	public class ImageWithShadowRenderer : ImageRenderer
	{
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this.Layer.ShadowOffset = new System.Drawing.SizeF(0, 3);
			this.Layer.ShadowColor = UIColor.Black.CGColor;
			this.Layer.ShadowRadius = 2.0f;
			this.Layer.ShadowOpacity = 0.5f;
		}
	}
}

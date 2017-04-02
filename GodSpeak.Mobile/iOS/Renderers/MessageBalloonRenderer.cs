using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;
using ObjCRuntime;
using CoreGraphics;
using Foundation;
using CoreAnimation;
using System.Linq;

[assembly: ExportRendererAttribute(typeof(MessageBalloon), typeof(MessageBalloonRenderer))]
namespace GodSpeak.iOS
{
	public class MessageBalloonRenderer : ViewRenderer
	{
		private CAGradientLayer GradientLayer
		{
			get { return (CAGradientLayer)Layer; }
		}

		public MessageBalloonRenderer()
		{
			GradientLayer.StartPoint = new CGPoint(0.5, 0);
			GradientLayer.EndPoint = new CGPoint(0.5, 1);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				Update();
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == GradientBoxView.ColorsProperty.PropertyName)
			{
				Update();
			}
		}

		private void Update()
		{
			if (Element == null)
				return;

			var colors = new Xamarin.Forms.Color[] {Xamarin.Forms.Color.FromRgb(1, 168, 248), Xamarin.Forms.Color.FromRgb(17, 164, 244)};
			GradientLayer.Colors = colors.Select(s => s.ToCGColor()).ToArray();
		}

		[Export("layerClass")]
		public static Class LayerClass()
		{
			return new Class(typeof(CAGradientLayer));
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			this.Layer.MasksToBounds = false;
			this.Layer.CornerRadius = 10.0f;
			this.Layer.BackgroundColor = ColorHelper.Primary.ToCGColor();
			this.Layer.ShadowOffset = new System.Drawing.SizeF(0, 2);
			this.Layer.ShadowColor = UIColor.Black.CGColor;
			this.Layer.ShadowRadius = 2.0f;
			this.Layer.ShadowOpacity = 0.5f;
		}
	}
}

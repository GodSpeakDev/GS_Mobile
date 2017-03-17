using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomLabel), typeof(CustomLabelRenderer))]

namespace GodSpeak.iOS
{
	public class CustomLabelRenderer : LabelRenderer
	{
		public CustomLabel CustomLabel
		{
			get { return Element as CustomLabel; }
		}

		public CustomLabelRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			SetFontWeight();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomLabel.FontWeightProperty.PropertyName)
			{
				SetFontWeight();
			}
		}

		private void SetFontWeight()
		{
			switch (this.CustomLabel.FontWeight)
			{
				case CustomLabel.FontWeights.Light:
					this.Control.Font = UIFont.SystemFontOfSize((nfloat)CustomLabel.FontSize, UIFontWeight.Light);
					break;
				case CustomLabel.FontWeights.Regular:
					this.Control.Font = UIFont.SystemFontOfSize((nfloat)CustomLabel.FontSize, UIFontWeight.Regular);
					break;
				case CustomLabel.FontWeights.Semibold:
					this.Control.Font = UIFont.SystemFontOfSize((nfloat)CustomLabel.FontSize, UIFontWeight.Semibold);
					break;
				case CustomLabel.FontWeights.Bold:
					this.Control.Font = UIFont.SystemFontOfSize((nfloat)CustomLabel.FontSize, UIFontWeight.Bold);
					break;
				case CustomLabel.FontWeights.Heavy:
					this.Control.Font = UIFont.SystemFontOfSize((nfloat)CustomLabel.FontSize, UIFontWeight.Heavy);
					break;
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
		}
	}
}

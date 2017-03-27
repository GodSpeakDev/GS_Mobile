using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;

[assembly: ExportRendererAttribute(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace GodSpeak.Droid
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
			SetFontWeight();
		}

		private void SetFontWeight()
		{
			if (this.Control == null)
				return;

			this.Control.SetTypeface(null, this.CustomLabel.GetFont());
		}
	}
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace GodSpeak.iOS
{
	public class CustomPickerRenderer : PickerRenderer
	{
		public CustomPicker CustomPicker
		{
			get { return Element as CustomPicker; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			SetBorderColor();
			SetBorderFrame();
			SetTextAligment();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomPicker.OutlineColorProperty.PropertyName)
			{
				SetBorderColor();
			}
		}

		private void SetTextAligment()
		{
			this.Control.TextAlignment = UITextAlignment.Center;
		}

		private void SetBorderColor()
		{			
			this.Control.Layer.BorderColor = CustomPicker.OutlineColor.ToCGColor();

			this.Control.EditingDidEnd += (sender, e) =>
			{
				CustomPicker.IsFocused = false;
			};

			this.Control.EditingDidBegin += (sender, e) =>
			{
				CustomPicker.IsFocused = true;
			};
		}

		private void SetBorderFrame()
		{
			this.Control.Layer.BorderWidth = 1;
			this.Control.Layer.MasksToBounds = true;
			this.Control.Layer.CornerRadius = 5.0f;
		}
	}
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]
namespace GodSpeak.iOS
{
	public class CustomTimePickerRenderer : TimePickerRenderer
	{
		public CustomTimePicker CustomPicker
		{
			get { return Element as CustomTimePicker; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
			base.OnElementChanged(e);

			SetBorderColor();
			SetBorderFrame();
			SetTextAligment();
			SetFontWeight();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomTimePicker.OutlineColorProperty.PropertyName)
			{
				SetBorderColor();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var uiimage = new UIImage("arrow_down.png");
			var image = new UIImageView()
			{

			};
			image.Image = uiimage;

			var width = uiimage.Size.Width * 0.8;
			var height = uiimage.Size.Height * 0.8;

			var x = this.Frame.Width - width - 10;
			var y = (this.Frame.Height - height) / 2;

			image.Frame = new CoreGraphics.CGRect(x, y, width, height);

			this.AddSubview(image);
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

		private void SetFontWeight()
		{
			if (this.Control == null || this.CustomPicker == null)
				return;

			this.Control.Font = this.CustomPicker.GetUIFont();
		}
	}
}

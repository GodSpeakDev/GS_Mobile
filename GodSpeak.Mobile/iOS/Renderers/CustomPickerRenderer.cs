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
			SetFontWeight();
			SetBackgroundColor();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomPicker.OutlineColorProperty.PropertyName)
			{
				SetBorderColor();
			}
			else if (e.PropertyName == CustomPicker.FontSizeProperty.PropertyName)
			{
				SetFontWeight();
			}

			SetBackgroundColor();
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

		private void SetBackgroundColor()
		{
			if (this.Control != null)
			{
				this.Control.BackgroundColor = this.CustomPicker.BackgroundColor.ToUIColor();
			}
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

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace GodSpeak.Droid
{
	public class CustomPickerRenderer : PickerRenderer
	{
		private GradientDrawable _drawable;
		private GradientDrawable Drawable
		{
			get
			{
				if (_drawable == null)
				{
					_drawable = new GradientDrawable();
					this.Control.SetBackground(_drawable);
				}

				return _drawable;
			}
		}

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

			SetBorderColor();
			SetFontWeight();
			SetTextAligment();
			SetBackgroundColor();

			if (e.PropertyName == "IsFocused")
			{
				CustomPicker.IsFocused = this.Control.IsFocused;
			}
			else if (e.PropertyName == "BackgroundColor")
			{
				SetBackgroundColor();
			}
		}

		private void SetTextAligment()
		{
			this.Control.Gravity = Android.Views.GravityFlags.CenterHorizontal;
		}

		private void SetBorderColor()
		{
			var customEntry = this.Element as CustomPicker;
			if (this.Control != null && customEntry != null)
			{
				Drawable.SetStroke(2, this.CustomPicker.OutlineColor.ToAndroid());
			}
		}

		private void SetBackgroundColor()
		{
			this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.CustomPicker.BackgroundColor.ToAndroid());
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{
				Drawable.SetCornerRadius(15);
			}
		}

		private void SetFontWeight()
		{
			if (this.Control == null || this.CustomPicker == null)
				return;

			this.Control.SetTypeface(null, this.CustomPicker.GetFont());
		}
	}
}

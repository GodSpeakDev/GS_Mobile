using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace GodSpeak.Droid
{
	public class CustomButtonRenderer : ButtonRenderer
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

		public CustomButton CustomButton
		{
			get { return Element as CustomButton; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			SetBorderFrame();
			SetBackgroundColor();
			SetBorderColor();
			SetTextAlignment();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetBackgroundColor();
			SetBorderColor();
            SetTextAlignment();

			if (e.PropertyName == "BackgroundColor")
			{
				SetBackgroundColor();
			}
		}

		private void SetBorderColor()
		{
			var customButton = this.Element as CustomButton;
			if (this.Control != null && customButton != null)
			{
				Drawable.SetStroke(2, this.CustomButton.BorderColor.ToAndroid());
			}
		}

		private void SetBackgroundColor()
		{
			this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.CustomButton.BackgroundColor.ToAndroid());
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{
				Drawable.SetCornerRadius(15);
			}
		}

		private void SetTextAlignment()
		{
			if (CustomButton.TextAlignment == Xamarin.Forms.TextAlignment.Start)
			{
				this.Control.Gravity = Android.Views.GravityFlags.Left | Android.Views.GravityFlags.CenterVertical;
			}
			else if (CustomButton.TextAlignment == Xamarin.Forms.TextAlignment.Center)
			{
                this.Control.Gravity = Android.Views.GravityFlags.CenterHorizontal | Android.Views.GravityFlags.CenterVertical;
			}
			else if (CustomButton.TextAlignment == Xamarin.Forms.TextAlignment.End)
			{
                this.Control.Gravity = Android.Views.GravityFlags.Right | Android.Views.GravityFlags.CenterVertical;
			}
		}
	}
}

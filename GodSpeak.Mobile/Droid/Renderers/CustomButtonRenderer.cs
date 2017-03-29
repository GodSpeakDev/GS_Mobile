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
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetBackgroundColor();

			if (e.PropertyName == "BackgroundColor")
			{
				SetBackgroundColor();
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
	}
}

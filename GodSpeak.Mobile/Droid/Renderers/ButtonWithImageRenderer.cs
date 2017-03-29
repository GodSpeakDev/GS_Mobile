using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(ButtonWithImage), typeof(ButtonWithImageRenderer))]
namespace GodSpeak.Droid
{
	public class ButtonWithImageRenderer : ViewRenderer
	{
		private GradientDrawable _drawable;
		private GradientDrawable Drawable
		{
			get
			{
				if (_drawable == null)
				{
					_drawable = new GradientDrawable();
					this.SetBackground(_drawable);
				}

				return _drawable;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
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
			//this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.Element.BackgroundColor.ToAndroid());		
		}

		private void SetBorderFrame()
		{			
			Drawable.SetCornerRadius(15);		
		}
	}
}

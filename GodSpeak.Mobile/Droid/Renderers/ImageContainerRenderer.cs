using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(ImageContainer), typeof(ImageContainerRenderer))]
namespace GodSpeak.Droid
{
	public class ImageContainerRenderer : ViewRenderer
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
			SetBorderColor();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetBorderColor();
		}

		private void SetBorderColor()
		{			
			Drawable.SetStroke(2, ColorHelper.OutlinePlaceHolder.ToAndroid());		
		}

		private void SetBorderFrame()
		{			
			Drawable.SetCornerRadius(15);		
		}
	}
}

﻿using System;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using Xamarin.Forms;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace GodSpeak.Droid
{
	public class CustomImageRenderer : ImageRenderer
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

		public CustomImage CustomImage
		{
			get { return Element as CustomImage; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			SetBorderColor();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetBorderColor();
		}

		private void SetBorderColor()
		{
			var customImage = this.Element as CustomImage;
			if (this.Control != null && customImage != null)
			{
				Drawable.SetStroke(2, this.CustomImage.BorderColor.ToAndroid());
			}
		}
	}
}

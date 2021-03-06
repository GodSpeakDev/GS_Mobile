﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(PlusButton), typeof(PlusButtonRenderer))]
namespace GodSpeak.Droid
{
	public class PlusButtonRenderer : ButtonRenderer
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

		public PlusButtonRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			SetBackgroundColor();
            SetAlignment();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			SetBackgroundColor();
			SetAlignment();
		}

		private void SetAlignment() 
		{
			this.Control.SetPadding(0, 0, 0, 0);
		}

		private void SetBackgroundColor()
		{
			this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(Xamarin.Forms.Color.Transparent.ToAndroid());
		}
	}
}

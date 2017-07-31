using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Diagnostics;

[assembly: ExportRendererAttribute(typeof(MenuBackground), typeof(MenuBackgroundRenderer))]
namespace GodSpeak.Droid
{
	public class MenuBackgroundRenderer : VisualElementRenderer<MenuBackground>
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

		protected override void OnElementChanged(ElementChangedEventArgs<MenuBackground> e)
		{
			base.OnElementChanged(e);
            UpdateCornerRadius();
            SetBackgroundColor();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			UpdateCornerRadius();
            SetBackgroundColor();
		}

		private void UpdateCornerRadius()
		{
			if (Drawable == null)
				return;

			var density = Context.Resources.DisplayMetrics.Density;
			Drawable.SetCornerRadius(50 * density);

            System.Diagnostics.Debug.WriteLine("this.Element.WidthRequest: {0}", this.Element.WidthRequest);
            System.Diagnostics.Debug.WriteLine("this.Width: {0}", this.Width);
		}

		private void SetBackgroundColor()
		{
			//this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.Element.BackgroundColor.ToAndroid());
		}

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            UpdateCornerRadius();
        }
	}
}

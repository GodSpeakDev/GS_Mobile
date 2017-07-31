using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(MenuItemView), typeof(MenuItemViewRenderer))]
namespace GodSpeak.Droid
{
	public class MenuItemViewRenderer : VisualElementRenderer<MenuItemView>
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

		protected override void OnElementChanged(ElementChangedEventArgs<MenuItemView> e)
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
		}

		private void SetBackgroundColor()
		{			
			Drawable.SetColor(this.Element.BackgroundColor.ToAndroid());
		}
	}
}

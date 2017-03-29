using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(PlusMinusButtonView), typeof(PlusMinusButtonViewRenderer))]
namespace GodSpeak.Droid
{
	public class PlusMinusButtonViewRenderer : ViewRenderer
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

			SetBackground();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetBackground();
		}

		private void SetBackground()
		{
			Drawable.SetCornerRadius(15);
			Drawable.SetStroke(3, ColorHelper.Secondary.ToAndroid());
			Drawable.SetColor(Android.Graphics.Color.Transparent);
		}
	}
}

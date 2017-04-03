using System;
using Xamarin.Forms;
using GodSpeak;
using GodSpeak.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Linq;

[assembly: ExportRendererAttribute(typeof(MessageBalloon), typeof(MessageBalloonRenderer))]
namespace GodSpeak.Droid
{
	public class MessageBalloonRenderer : ViewRenderer
	{
		private GradientDrawable _drawable;

		protected override void OnElementChanged(ElementChangedEventArgs<GradientBoxView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_drawable = new GradientDrawable();
				_drawable.SetOrientation(GradientDrawable.Orientation.TopBottom);

				this.SetBackground(_drawable);
			}

			UpdateColors();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			this.SetLayerType(Android.Views.LayerType.Hardware, null);
			base.OnElementPropertyChanged(sender, e);

			UpdateColors();
			UpdateCornerRadius();
			this.SetLayerType(Android.Views.LayerType.None, null);
		}

		private void UpdateCornerRadius()
		{
			if (_drawable == null)
				return;

			var radius = GetRadius(15);

			_drawable.SetCornerRadii(new float[] { radius, radius, radius, radius, radius, radius, radius, radius });
		}

		private void UpdateColors()
		{
			if (_drawable == null)
				return;
			
			var colors = new Xamarin.Forms.Color[] { Element.BackgroundColor, Element.BackgroundColor };

			_drawable.SetColors(colors.Select(s => s.ToAndroid().ToArgb()).ToArray());
		}

		private float GetRadius(double radius)
		{
			radius = (float)Math.Round(Width * radius / 260);

			if (radius <= 0)
			{
				radius = 0;
			}

			return (float)radius;
		}
	}
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace GodSpeak.Droid
{
	public class CustomEditorRenderer : EditorRenderer
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

		public CustomEditor CustomEditor
		{
			get { return Element as CustomEditor; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

            SetBorderFrame();
			SetBackgroundColor();
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{				
				Drawable.SetCornerRadius(15);
			}
		}

		private void SetBackgroundColor()
		{
			this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.CustomEditor.BackgroundColor.ToAndroid());
		}
	}
}

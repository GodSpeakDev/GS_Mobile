using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GodSpeak;
using GodSpeak.Droid;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace GodSpeak.Droid
{
	public class CustomEntryRenderer : EntryRenderer, Android.Views.View.IOnFocusChangeListener
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

		public CustomEntry CustomEntry
		{
			get { return Element as CustomEntry; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			SetBorderFrame();
			SetBorderColor();
			SetTextAligment();
			SetFontWeight();
			SetBackgroundColor();
		}

		private void SetBackgroundColor()
		{			
			this.SetBackgroundColor(Android.Graphics.Color.Transparent);
			Drawable.SetColor(this.CustomEntry.BackgroundColor.ToAndroid());
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetBorderColor();
			SetFontWeight();
			SetTextAligment();
			SetBackgroundColor();

			if (e.PropertyName == "IsFocused")
			{
				CustomEntry.IsFocused = this.Control.IsFocused;
			}
			else if (e.PropertyName == "BackgroundColor")
			{				
				SetBackgroundColor();
			}
		}

		private void SetTextAligment()
		{
			if (this.Control != null)
			{
				this.Control.Gravity = Android.Views.GravityFlags.CenterHorizontal;
			}	
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{
				Drawable.SetCornerRadius(15);
			}
		}

		private void SetBorderColor()
		{
			var customEntry = this.Element as CustomEntry;
			if (this.Control != null && customEntry != null)
			{
				Drawable.SetStroke(2, this.CustomEntry.OutlineColor.ToAndroid());

				this.OnFocusChangeListener = this;
			}
		}

		public void OnFocusChange(View v, bool hasFocus)
		{
			
		}

		private void SetFontWeight()
		{
			if (this.Control == null || this.CustomEntry == null)
				return;

			this.Control.SetTypeface(null, this.CustomEntry.GetFont());			                         
		}

		//private class CustomOnFocusChangeListener : Android.Views.View.IOnFocusChangeListener
		//{
		//	public IntPtr Handle
		//	{
		//		get { return IntPtr.Zero;}
		//	}

		//	public void OnFocusChange(View v, bool hasFocus)
		//	{
				
		//	}
		//}
	}
}

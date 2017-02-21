using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using GodSpeak;
using GodSpeak.iOS;

[assembly: ExportRendererAttribute(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace GodSpeak.iOS
{
	public class CustomEntryRenderer : EntryRenderer
	{
		public CustomEntry CustomEntry
		{
			get { return Element as CustomEntry;}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			SetBorderFrame();
			SetBorderColor();
			SetTextAligment();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomEntry.OutlineColorProperty.PropertyName)
			{
				SetBorderColor();
			}				
		}

		private void SetTextAligment()
		{
			this.Control.TextAlignment = UITextAlignment.Center;
		}

		private void SetBorderFrame()
		{
			this.Control.Layer.BorderWidth = 1;
			this.Control.Layer.MasksToBounds = true;
			this.Control.Layer.CornerRadius = 5.0f;
		}

		private void SetBorderColor()
		{
			var customEntry = this.Element as CustomEntry;
			this.Control.Layer.BorderColor = customEntry.OutlineColor.ToCGColor();

			this.Control.EditingDidEnd += (sender, e) => 
			{
				CustomEntry.IsFocused = false;
			};

			this.Control.EditingDidBegin += (sender, e) => 
			{
				CustomEntry.IsFocused = true;
			};
		}
	}
}

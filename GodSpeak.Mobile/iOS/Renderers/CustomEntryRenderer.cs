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
			SetFontWeight();
			SetHasAutoComplete();
			SetBackgroundColor();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			SetBorderColor();
			SetFontWeight();
			SetHasAutoComplete();
			SetBackgroundColor();
		}

		private void SetTextAligment()
		{
			if (this.Control != null)
			{
				this.Control.TextAlignment = UITextAlignment.Center;
			}
		}

		private void SetBackgroundColor()
		{
			if (this.Control != null && this.CustomEntry != null)
			{
				this.Control.BackgroundColor = this.CustomEntry.BackgroundColor.ToUIColor();
			}
		}

		private void SetBorderFrame()
		{
			if (this.Control != null)
			{
				this.Control.Layer.BorderWidth = 1;
				this.Control.Layer.MasksToBounds = true;
				this.Control.Layer.CornerRadius = 5.0f;
			}
		}

		private void SetBorderColor()
		{
			var customEntry = this.Element as CustomEntry;
			if (this.Control != null && customEntry != null)
			{
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

		private void SetFontWeight()
		{
			if (this.Control == null || this.CustomEntry == null)
				return;

			this.Control.Font = this.CustomEntry.GetUIFont();
		}

		private void SetHasAutoComplete()
		{
			if (this.Control == null || this.CustomEntry == null)
				return;

			this.Control.AutocorrectionType = CustomEntry.HasAutoComplete ? UITextAutocorrectionType.Yes : UITextAutocorrectionType.No;
		}
	}
}

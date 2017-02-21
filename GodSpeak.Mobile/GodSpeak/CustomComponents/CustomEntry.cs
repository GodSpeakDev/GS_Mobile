using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomEntry : Entry
	{
		public static readonly BindableProperty OutlineColorProperty =
			BindableProperty.Create<CustomEntry, Color>(
				p => p.OutlineColor, Color.White, BindingMode.TwoWay, propertyChanged: OnOutlineColorChanged);

		public Color OutlineColor
		{
			get { return (Color)this.GetValue(OutlineColorProperty); }
			set { this.SetValue(OutlineColorProperty, value); }
		}

		private static void OnOutlineColorChanged(BindableObject bindable, Color oldvalue, Color newValue)
		{
			var entry = (CustomEntry)bindable;
			entry.OutlineColor = newValue;
		}

		public static readonly BindableProperty HasErrorProperty = BindableProperty.Create<CustomEntry, bool>(
				p => p.HasError, false, BindingMode.TwoWay, propertyChanged: HasErrorChanged);

		public bool HasError
		{
			get { return (bool)this.GetValue(HasErrorProperty); }
			set { this.SetValue(HasErrorProperty, value); }
		}

		private static void HasErrorChanged(BindableObject bindable, bool oldvalue, bool newValue)
		{
			var entry = (CustomEntry)bindable;
			entry.HasError = newValue;
		}

		private bool _isFocused;
		public new bool IsFocused
		{
			get { return _isFocused;}
			set 
			{ 
				_isFocused = value;
				SetUI();
			}
		}

		public CustomEntry()
		{
			HeightRequest = 35;
			PlaceholderColor = ColorHelper.TextInputPlaceHolder;
			SetUI();
		}

		private void SetUI()
		{
			SetOutlineColor();
			SetBackgroundColor();
			SetTextColor();
			SetFont();
		}

		private void SetOutlineColor()
		{
			if (IsFocused)
			{
				OutlineColor = ColorHelper.Secondary;
			}
			else
			{
				OutlineColor = string.IsNullOrEmpty(Text) ? ColorHelper.OutlinePlaceHolder : ColorHelper.Secondary;
			}
		}

		private void SetTextColor()
		{
			if (IsFocused)
			{
				TextColor = ColorHelper.TextInputFocusedText;
			}
			else
			{
				TextColor = ColorHelper.Secondary;
			}
		}

		private void SetBackgroundColor()
		{
			if (IsFocused)
			{				
				BackgroundColor = ColorHelper.Secondary;
			}
			else
			{				
				BackgroundColor = ColorHelper.DarkGray;
			}
		}

		private void SetFont()
		{
			FontAttributes = IsFocused ? FontAttributes.Bold : FontAttributes.None;
		}
	}
}

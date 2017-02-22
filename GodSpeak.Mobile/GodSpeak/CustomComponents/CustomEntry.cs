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
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				OutlineColor = ColorHelper.Warning;
			}
			else if (IsFocused)
			{
				OutlineColor = ColorHelper.Secondary;
			}
			else if (!IsEnabled)
			{
				OutlineColor = ColorHelper.OutlinePlaceHolder;
			}
			else
			{
				OutlineColor = string.IsNullOrEmpty(Text) ? ColorHelper.OutlinePlaceHolder : ColorHelper.Secondary;
			}
		}

		private void SetTextColor()
		{
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				TextColor = ColorHelper.Secondary;
			}
			else if (IsFocused)
			{
				TextColor = ColorHelper.TextInputFocusedText;
			}
			else if (!IsEnabled)
			{
				TextColor = ColorHelper.TextInputDisabledText;
			}
			else if (!string.IsNullOrEmpty(Text))
			{
				TextColor = ColorHelper.Secondary;
			}
			else
			{
				TextColor = ColorHelper.TextInputPlaceHolder;
			}
		}

		private void SetBackgroundColor()
		{
			if (!string.IsNullOrEmpty(Text) && HasError)
			{
				BackgroundColor = ColorHelper.Warning;
			}
			else if (IsFocused)
			{
				BackgroundColor = ColorHelper.Secondary;
			}
			else if (!IsEnabled)
			{
				BackgroundColor = ColorHelper.OutlinePlaceHolder;
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

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}

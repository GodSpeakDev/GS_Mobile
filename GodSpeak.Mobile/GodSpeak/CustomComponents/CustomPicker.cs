using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomPicker : Picker, ICustomFont
	{
		public static readonly BindableProperty OutlineColorProperty =
			BindableProperty.Create<CustomPicker, Color>(
				p => p.OutlineColor, Color.White, BindingMode.TwoWay, propertyChanged: OnOutlineColorChanged);

		public Color OutlineColor
		{
			get { return (Color)this.GetValue(OutlineColorProperty); }
			set { this.SetValue(OutlineColorProperty, value); }
		}

		private static void OnOutlineColorChanged(BindableObject bindable, Color oldvalue, Color newValue)
		{
			var entry = (CustomPicker)bindable;
			entry.OutlineColor = newValue;
		}

		public static readonly BindableProperty HasEmptyValueProperty =
			BindableProperty.Create<CustomPicker, bool>(
				p => p.HasEmptyValue, true, BindingMode.TwoWay, propertyChanged: HasEmptyValueChanged);

		public bool HasEmptyValue
		{
			get { return (bool)this.GetValue(HasEmptyValueProperty); }
			set { this.SetValue(HasEmptyValueProperty, value); }
		}

		private static void HasEmptyValueChanged(BindableObject bindable, bool oldvalue, bool newValue)
		{
			var entry = (CustomPicker)bindable;
			entry.HasEmptyValue = newValue;
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<CustomPicker, double>(
				p => p.FontSize, 14, BindingMode.TwoWay, propertyChanged: FontSizeChanged);

		public double FontSize
		{
			get { return (double)this.GetValue(FontSizeProperty); }
			set { this.SetValue(FontSizeProperty, value); }
		}

		private static void FontSizeChanged(BindableObject bindable, double oldvalue, double newValue)
		{
			var entry = (CustomPicker)bindable;
			entry.FontSize = newValue;
		}

		public static readonly BindableProperty FontWeightProperty =
			BindableProperty.Create<CustomPicker, GodSpeak.FontWeight>(
				p => p.FontWeight, GodSpeak.FontWeight.Regular, BindingMode.TwoWay, propertyChanged: OnFontWeightChanged);

		public GodSpeak.FontWeight FontWeight
		{
			get { return (GodSpeak.FontWeight)this.GetValue(FontWeightProperty); }
			set { this.SetValue(FontWeightProperty, value); }
		}

		private static void OnFontWeightChanged(BindableObject bindable, GodSpeak.FontWeight oldvalue, GodSpeak.FontWeight newValue)
		{
			var entry = (CustomPicker)bindable;
			entry.FontWeight = newValue;
		}

		private bool _isFocused;
		public new bool IsFocused
		{
			get { return _isFocused; }
			set
			{
				_isFocused = value;
				SetUI();
			}
		}

		public CustomPicker()
		{
			HeightRequest = 35;
			SetUI();
		}

		private void SetUI()
		{
			SetOutlineColor();
			SetBackgroundColor();
			SetTextColor();
		}

		private void SetOutlineColor()
		{
			if (IsFocused)
			{
				OutlineColor = ColorHelper.Secondary;
			}
			else
			{				
				OutlineColor = this.SelectedIndex == 0 && HasEmptyValue ? ColorHelper.OutlinePlaceHolder : ColorHelper.Secondary;
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
				BackgroundColor = Color.Transparent;
			}
		}

		private void SetTextColor()
		{			
			if (IsFocused)
			{
				TextColor = ColorHelper.TextInputFocusedText;
			}
			else if (SelectedIndex > 0 || !HasEmptyValue)
			{
				TextColor = ColorHelper.Secondary;
			}
			else
			{
				TextColor = ColorHelper.TextInputPlaceHolder;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}

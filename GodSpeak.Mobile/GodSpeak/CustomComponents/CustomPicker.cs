using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomPicker : Picker
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

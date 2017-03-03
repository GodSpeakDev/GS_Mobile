using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomTimePicker : TimePicker
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

		public CustomTimePicker()
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
				OutlineColor = ColorHelper.Secondary;
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
			else
			{
				TextColor = ColorHelper.Secondary;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}

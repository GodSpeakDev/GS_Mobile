using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomTimePicker : TimePicker, ICustomFont
	{
		public static readonly BindableProperty ElementStateProperty =
			BindableProperty.Create<CustomTimePicker, ElementState>(
				p => p.ElementState, ElementState.NotFocused, BindingMode.TwoWay, propertyChanged: OnElementStateChanged);

		public ElementState ElementState
		{
			get { return (ElementState)this.GetValue(ElementStateProperty); }
			set { this.SetValue(ElementStateProperty, value); }
		}

		private static void OnElementStateChanged(BindableObject bindable, ElementState oldvalue, ElementState newValue)
		{
			var entry = (CustomTimePicker)bindable;
			entry.ElementState = newValue;
		}

		public static readonly BindableProperty OutlineColorProperty =
			BindableProperty.Create<CustomTimePicker, Color>(
				p => p.OutlineColor, Color.White, BindingMode.TwoWay, propertyChanged: OnOutlineColorChanged);

		public Color OutlineColor
		{
			get { return (Color)this.GetValue(OutlineColorProperty); }
			set { this.SetValue(OutlineColorProperty, value); }
		}

		private static void OnOutlineColorChanged(BindableObject bindable, Color oldvalue, Color newValue)
		{
			var entry = (CustomTimePicker)bindable;
			entry.OutlineColor = newValue;
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<CustomTimePicker, double>(
				p => p.FontSize, 14, BindingMode.TwoWay, propertyChanged: FontSizeChanged);

		public double FontSize
		{
			get { return (double)this.GetValue(FontSizeProperty); }
			set { this.SetValue(FontSizeProperty, value); }
		}

		private static void FontSizeChanged(BindableObject bindable, double oldvalue, double newValue)
		{
			var entry = (CustomTimePicker)bindable;
			entry.FontSize = newValue;
		}

		public static readonly BindableProperty FontWeightProperty =
			BindableProperty.Create<CustomTimePicker, GodSpeak.FontWeight>(
				p => p.FontWeight, GodSpeak.FontWeight.Regular, BindingMode.TwoWay, propertyChanged: OnFontWeightChanged);

		public GodSpeak.FontWeight FontWeight
		{
			get { return (GodSpeak.FontWeight)this.GetValue(FontWeightProperty); }
			set { this.SetValue(FontWeightProperty, value); }
		}

		private static void OnFontWeightChanged(BindableObject bindable, GodSpeak.FontWeight oldvalue, GodSpeak.FontWeight newValue)
		{
			var entry = (CustomTimePicker)bindable;
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

		public CustomTimePicker()
		{
			HeightRequest = 42;
			SetUI();
		}

		private void SetUI()
		{
			if (IsFocused)
			{
				ElementState = ElementState.Focused;
			}
			else
			{
				ElementState = ElementState.NotFocused;
			}
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			SetUI();
		}
	}
}

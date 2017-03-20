using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomEntry : Entry, ICustomFont
	{
		public static readonly BindableProperty FontWeightProperty =
			BindableProperty.Create<CustomLabel, GodSpeak.FontWeight>(
				p => p.FontWeight, GodSpeak.FontWeight.Regular, BindingMode.TwoWay, propertyChanged: OnFontWeightChanged);

		public GodSpeak.FontWeight FontWeight
		{
			get { return (GodSpeak.FontWeight)this.GetValue(FontWeightProperty); }
			set { this.SetValue(FontWeightProperty, value); }
		}

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

		private static void OnFontWeightChanged(BindableObject bindable, GodSpeak.FontWeight oldvalue, GodSpeak.FontWeight newValue)
		{
			var entry = (CustomEntry)bindable;
			entry.FontWeight = newValue;
		}

		private bool _isFocused;
		public virtual new bool IsFocused
		{
			get { return _isFocused;}
			set 
			{ 
				_isFocused = value;
			}
		}

		public CustomEntry()
		{
			HeightRequest = 35;
		}
	}
}

using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomLabel : Label, ICustomFont
	{		
		public static readonly BindableProperty FontWeightProperty =
			BindableProperty.Create<CustomLabel, GodSpeak.FontWeight>(
				p => p.FontWeight, GodSpeak.FontWeight.Regular, BindingMode.TwoWay, propertyChanged: OnFontWeightChanged);

		public GodSpeak.FontWeight FontWeight
		{
			get { return (GodSpeak.FontWeight)this.GetValue(FontWeightProperty); }
			set { this.SetValue(FontWeightProperty, value); }
		}

		private static void OnFontWeightChanged(BindableObject bindable, GodSpeak.FontWeight oldvalue, GodSpeak.FontWeight newValue)
		{
			var entry = (CustomLabel)bindable;
			entry.FontWeight = newValue;
		}

		public CustomLabel()
		{
		}
	}
}

using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomLabel : Label
	{
		public enum FontWeights
		{
			Regular,
			Bold,
			Semibold,
			Light,
			Heavy
		}

		public static readonly BindableProperty FontWeightProperty =
			BindableProperty.Create<CustomLabel, FontWeights>(
				p => p.FontWeight, FontWeights.Regular, BindingMode.TwoWay, propertyChanged: OnFontWeightChanged);

		public FontWeights FontWeight
		{
			get { return (FontWeights)this.GetValue(FontWeightProperty); }
			set { this.SetValue(FontWeightProperty, value); }
		}

		private static void OnFontWeightChanged(BindableObject bindable, FontWeights oldvalue, FontWeights newValue)
		{
			var entry = (CustomLabel)bindable;
			entry.FontWeight = newValue;
		}

		public CustomLabel()
		{
		}
	}
}

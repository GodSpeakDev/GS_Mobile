using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomButton : Button
	{
		public static readonly BindableProperty TextAlignmentProperty =
			BindableProperty.Create<CustomButton, TextAlignment>(
				p => p.TextAlignment, TextAlignment.Center, BindingMode.TwoWay, propertyChanged: OnTextAlignmentChanged);

		public TextAlignment TextAlignment
		{
			get { return (TextAlignment)this.GetValue(TextAlignmentProperty); }
			set { this.SetValue(TextAlignmentProperty, value); }
		}

		private static void OnTextAlignmentChanged(BindableObject bindable, TextAlignment oldvalue, TextAlignment newValue)
		{
			var entry = (CustomButton)bindable;
			entry.TextAlignment = newValue;
		}

		public CustomButton()
		{
		}
	}
}

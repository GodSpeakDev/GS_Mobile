using System;
using Xamarin.Forms;
using System.Globalization;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class SelectedVerseToFontAttributesConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isSelected = (bool)value;
			return isSelected ? FontAttributes.Bold : FontAttributes.None;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

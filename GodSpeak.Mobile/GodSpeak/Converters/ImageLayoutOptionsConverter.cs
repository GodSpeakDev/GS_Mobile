using System;
using Xamarin.Forms;
using System.Globalization;

namespace GodSpeak
{
	public class ImageLayoutOptionsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || (value is string && ((string)value).Contains("placeholder")))
			{
				return LayoutOptions.Center;
			}

			return LayoutOptions.Fill;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

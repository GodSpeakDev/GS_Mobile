using System;
using Xamarin.Forms;
using System.Globalization;

namespace GodSpeak
{
	public class WeekdayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string[] names = culture.DateTimeFormat.DayNames;
			return names[(int)value];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

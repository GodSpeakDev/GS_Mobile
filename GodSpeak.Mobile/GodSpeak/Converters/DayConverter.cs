using System;
using Xamarin.Forms;
using System.Globalization;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class DayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var date = (DateTime)value;

			if (DateTime.Today == date.Date)
			{
				return Text.Today;
			}
			else if (DateTime.Today.AddDays(-1) == date.Date)
			{
				return Text.Yesterday;
			}
			if (DateTime.Now.AddDays(-7).Date < date.Date)
			{
				return date.ToString("dddd");
			}
			else
			{
				return date.ToString("d");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

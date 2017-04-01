using System;
using Xamarin.Forms;
using System.Globalization;

namespace GodSpeak
{
	public class NotMinDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var date = (DateTime)value;
				return date != DateTime.MinValue && date != DateTime.MinValue.AddDays(1);
			}
			catch (Exception ex)
			{
				return true;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

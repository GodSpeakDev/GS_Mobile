using System;
using Xamarin.Forms;
using System.Globalization;
using System.IO;

namespace GodSpeak
{
	public class ImagePaddingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;
			if (str == "profile_placeholder.png")
			{
				return new Thickness(15);
			}

			return new Thickness(0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

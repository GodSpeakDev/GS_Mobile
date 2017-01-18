using System;
using Xamarin.Forms;
using System.Globalization;
using System.IO;

namespace GodSpeak
{
	public class ImageObjectToImageSourceConverter : IValueConverter 	
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || value is string)
			{
				return value as string;
			}
			else if (value is byte[])
			{
				var source = ImageSource.FromStream(() => new MemoryStream(value as byte[]));
				return source;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

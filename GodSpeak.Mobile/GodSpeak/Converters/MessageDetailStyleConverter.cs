using System;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class MessageDetailStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var isSelected = (bool)value;
			return isSelected ? (Style)Application.Current.Resources["MessageDetailsTextInFocus"] : 
				                (Style)Application.Current.Resources["MessageDetailsTextOutOfFocus"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

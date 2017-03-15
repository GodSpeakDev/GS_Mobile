using System;
using Xamarin.Forms;
using System.Globalization;
using GodSpeak.Resources;

namespace GodSpeak
{
	public class PeopleGiftedTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var peopleGifted = (int) value;
			return peopleGifted > 0 ? string.Format(Text.HasGiftedPeopleText, peopleGifted) : Text.HasNotGiftedPeopleText;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

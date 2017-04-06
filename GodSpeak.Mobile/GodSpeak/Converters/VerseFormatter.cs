using System;
using System.Globalization;
using Xamarin.Forms;
namespace GodSpeak
{
    public class VerseFormatter : IValueConverter
    {
        public VerseFormatter ()
        {
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            text = text.Replace ("``", "\"");
            text = text.Replace ("*", "");
            text = text.Replace ("{", "");
            text = text.Replace ("}", "");
            return text;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException ();
        }
    }
}

using System;
using System.Globalization;
using Xamarin.Forms;
using System.Linq;
namespace GodSpeak
{
    public class TextTruncationConverter : IValueConverter
    {
        private const int maxWordCount = 30;

        public TextTruncationConverter ()
        {
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;

            var words = text.Split (' ').ToList ();

            if (words.Count < maxWordCount)
                return text;

            return string.Join (" ", words.GetRange (0, maxWordCount)) + "...";
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

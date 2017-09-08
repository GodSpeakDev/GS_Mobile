using System;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class InvitedClaimedFontSizeConverter : ImpactFontConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var impactedDays = value as ObservableCollection<ImpactDay>;
			var total = impactedDays.Sum(x => x.InvitesClaimed);
            return 35;
			return GetFontSizeForCount(total);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}	
	}
}

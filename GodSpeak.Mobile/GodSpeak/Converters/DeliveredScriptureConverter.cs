using System;
using Xamarin.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace GodSpeak
{
	public class DeliveredScriptureConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var impactedDays = value as ObservableCollection<ImpactDay>;
			return impactedDays.Sum(x => x.ScripturesDelivered);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

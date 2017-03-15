using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class CustomListView : ListView
	{
		public CustomListView()
		{
			ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{
				SelectedItem = null;
			};
		}
	}
}

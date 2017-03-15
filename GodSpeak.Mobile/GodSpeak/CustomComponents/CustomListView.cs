using System;
using Xamarin.Forms;
using GodSpeak.Extensions;

namespace GodSpeak
{
	public class CustomListView : ListView
	{
		public CustomListView()
		{
			this.DeselectOnTap();
		}
	}
}

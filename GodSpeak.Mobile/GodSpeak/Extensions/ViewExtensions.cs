using System;
using Xamarin.Forms;

namespace GodSpeak.Extensions
{
    public static class ViewExtensions
    {
        public static void DeselectOnTap (this ListView listView)
        {
            listView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
                listView.SelectedItem = null;
            };
        }
    }
}

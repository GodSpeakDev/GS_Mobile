using System;
using Android.Content;
using MvvmCross.Plugins.WebBrowser;
using Xamarin.Forms;
using Android.App;

namespace GodSpeak.Droid
{
	public class CustomDroidWebBrowserTask : IMvxWebBrowserTask
	{
		public void ShowWebPage(string url)
		{
			var uri = Android.Net.Uri.Parse(url);
			var intent = new Intent(Intent.ActionView, uri);
			(Forms.Context as Activity).StartActivity(intent);   
		}
	}
}

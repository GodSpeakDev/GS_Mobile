using System;
using Android.App;
using Android.Content;
using MvvmCross.Plugins.WebBrowser;
using Xamarin.Forms;

namespace GodSpeak.Droid.Services
{
    

public class GodSpeakWebBrowserTask : IMvxWebBrowserTask
{
    public GodSpeakWebBrowserTask ()
	{
	}

	public void ShowWebPage (string url)
	{
		var uri = Android.Net.Uri.Parse (url);
		var intent = new Intent (Intent.ActionView, uri);
		(Forms.Context as Activity).StartActivity (intent);
	}
    }
}

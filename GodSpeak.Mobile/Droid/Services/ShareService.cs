using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using GodSpeak.Resources;

namespace GodSpeak.Droid
{
	public class ShareService : IShareService
	{
		public void Share(string message)
		{
			var shareIntent = new Intent();
			shareIntent.SetAction(Intent.ActionSend);
			shareIntent.PutExtra(Intent.ExtraText, message);
			shareIntent.SetType("text/plain");

			(Forms.Context as Activity).StartActivity(Intent.CreateChooser(shareIntent, Text.ShareTitle));
		}
	}
}

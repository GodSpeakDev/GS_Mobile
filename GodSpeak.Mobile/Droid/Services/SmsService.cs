using System;
using Android.App;
using Android.Content;
using GodSpeak.Services;

namespace GodSpeak.Droid.Services
{
    public class SmsService : ISmsService
    {
        public void SendMessage(string message)
        {
			var uri = Android.Net.Uri.Parse("smsto:");
			var intent = new Intent(Intent.ActionSendto, uri);
			intent.PutExtra("sms_body", message);			

            (Xamarin.Forms.Forms.Context as Activity).StartActivity(intent);
        }
    }
}

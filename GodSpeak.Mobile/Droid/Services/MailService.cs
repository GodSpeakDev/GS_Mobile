using System;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;

namespace GodSpeak.Droid
{
	public class MailService : IMailService
	{
		public void SendMail(string[] to, string[] cc = null, string[] bcc = null, string subject = null, string body = "", string[] files = null)
		{			
			Intent emailIntent = new Intent(Intent.ActionSendMultiple);
			emailIntent.SetType("application/image");
			emailIntent.PutExtra(Intent.ExtraSubject, subject);
			emailIntent.PutExtra(Intent.ExtraText, body);
			emailIntent.PutExtra(Intent.ExtraEmail, to);
			emailIntent.PutExtra(Intent.ExtraCc, cc);
			emailIntent.PutExtra(Intent.ExtraBcc, bcc);

			var uris = new List <IParcelable>();
			if (files != null)
			{
				foreach (var file in files)
				{
					var uri = Android.Net.Uri.FromFile(new Java.IO.File(file));	
					uris.Add(uri);
				}

				emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
			}

			(Xamarin.Forms.Forms.Context as Activity).StartActivity(Intent.CreateChooser(emailIntent, "Send mail..."));
		}
	}
}

using System;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace GodSpeak.Droid
{
	public class MailService : IMailService
	{
		public void SendMail(string[] to, string[] cc = null, string[] bcc = null, string subject = null, string body = "")
		{			
			Intent emailIntent = new Intent(Intent.ActionSend);
			emailIntent.SetType("application/image");
			emailIntent.PutExtra(Intent.ExtraSubject, subject);
			emailIntent.PutExtra(Intent.ExtraText, body);
			emailIntent.PutExtra(Intent.ExtraEmail, to);
			emailIntent.PutExtra(Intent.ExtraCc, cc);
			emailIntent.PutExtra(Intent.ExtraBcc, bcc);

			(Xamarin.Forms.Forms.Context as Activity).StartActivity(Intent.CreateChooser(emailIntent, "Send mail..."));
		}
	}
}

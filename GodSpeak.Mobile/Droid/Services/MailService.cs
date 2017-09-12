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
        private FileService _fileService;

        public MailService (IFileService fileService)
        {
            _fileService = fileService as FileService;
        }

		public async void SendMail(string[] to, string[] cc = null, string[] bcc = null, string subject = null, string body = "", string[] files = null)
		{			
			Intent emailIntent = new Intent(Intent.ActionSendMultiple);
			emailIntent.SetType("text/plain");
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
                    var content = await _fileService.ReadTextAsync(file);

                    var filePieces = file.Split('/');
					var fileName = filePieces[filePieces.Length - 1];

                    var publicPath = _fileService.GetPublicFilePath(fileName);

                    await _fileService.DeleteFileAsync(publicPath);
                    await _fileService.WriteTextAsync(publicPath, content);

                    var javaFile = new Java.IO.File(publicPath);
					var uri = Android.Net.Uri.FromFile(javaFile);
					uris.Add(uri);
				}

				emailIntent.PutParcelableArrayListExtra(Intent.ExtraStream, uris);
			}

			(Xamarin.Forms.Forms.Context as Activity).StartActivity(Intent.CreateChooser(emailIntent, "Send mail..."));
        }
	}
}

using System;
using System.Threading.Tasks;
using MessageUI;
using UIKit;
using Xamarin.Forms;
using Foundation;

namespace GodSpeak.iOS
{
	public class MailService : IMailService
	{
		public void SendMail(string[] to, string[] cc = null, string[] bcc = null, string subject = null, string body = "", string[] files = null)
		{
			if (MFMailComposeViewController.CanSendMail)
			{
				var mailController = new MFMailComposeViewController();

				mailController.Finished += (object s, MFComposeResultEventArgs args) =>
				{
					args.Controller.DismissViewController(true, null);
				};

				mailController.SetSubject(subject);
				mailController.SetToRecipients(to);
				mailController.SetCcRecipients(cc);
				mailController.SetBccRecipients(bcc);
				mailController.SetMessageBody(body, false);

				if (files != null)
				{
					foreach (var file in files)
					{
						var filePieces = file.Split('/');
						var fileName = filePieces[filePieces.Length - 1];
						mailController.AddAttachmentData(NSData.FromFile(file), "text/plain", fileName);
					}
				}

				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailController, true, null);
			}
		}
	}
}

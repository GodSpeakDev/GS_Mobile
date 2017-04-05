using System;
using System.Threading.Tasks;
using MessageUI;
using UIKit;
using Xamarin.Forms;

namespace GodSpeak.iOS
{
	public class MailService : IMailService
	{
		public void SendMail(string[] to, string[] cc = null, string[] bcc = null, string subject = null, string body = "")
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

				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailController, true, null);
			}
		}
	}
}

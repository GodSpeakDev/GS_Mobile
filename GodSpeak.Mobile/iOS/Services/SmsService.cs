using System;
using GodSpeak.Services;
using MessageUI;
using UIKit;

namespace GodSpeak.iOS.Services
{
    public class SmsService : ISmsService
    {
        public void SendMessage(string message)
        {
            if (MFMessageComposeViewController.CanSendText)
            {
				var smsController = new MFMessageComposeViewController();

                smsController.Finished += (sender, e) => 
                {
                    e.Controller.DismissViewController(true, null);
                };

                smsController.Body = message;                				

				UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, null);
            }
        }
    }
}

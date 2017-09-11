using System;
using GodSpeak.Services;
using Android.App;
using HockeyApp.Android;
using Android.Content;

namespace GodSpeak.Droid.Services
{
    public class FeedbackService : IFeedbackService
    {
		private IMessageService _messageService;
		private IFileService _fileService;
		private IMailService _mailService;

		public FeedbackService(IMessageService messageService, IFileService fileService, IMailService mailService)
		{
			_messageService = messageService;
			_fileService = fileService;
			_mailService = mailService;
		}

        public void OpenFeedbackDialog ()
        {
			//var deliveredMessages = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath(_messageService.DeliveredMessagesFile)));
			//var upcomingMessages = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath(_messageService.UpcomingMessagesFile)));
			//var log = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath("Log.txt")));

			//FeedbackManager.ShowFeedbackActivity(Xamarin.Forms.Forms.Context, new Android.Net.Uri[] {deliveredMessages, upcomingMessages, log});

			_mailService.SendMail(
				to: new string[] { "support@givegodspeak.com" },
				subject: "[Android] GodSpeak Feedback", 
				files: new string[] 
				{
					_fileService.GetFilePath(_messageService.DeliveredMessagesFile),
					_fileService.GetFilePath(_messageService.UpcomingMessagesFile),
					_fileService.GetFilePath("Log.txt")
				});
        }
    }
}

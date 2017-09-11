using System;
using GodSpeak.Services;
using HockeyApp.iOS;
using Foundation;
namespace GodSpeak.iOS.Services
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
			//var deliveredMessages = NSData.FromFile(_fileService.GetFilePath(_messageService.DeliveredMessagesFile));
			//var upcomingMessages = NSData.FromFile(_fileService.GetFilePath(_messageService.UpcomingMessagesFile));
			//var log = NSData.FromFile(_fileService.GetFilePath("Log.txt"));

			//var data = new NSObject[] { deliveredMessages, upcomingMessages, log};
			//BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackComposeViewWithPreparedItems(data);

			_mailService.SendMail(
				to: new string[] { "support@givegodspeak.com" },
				subject: "[iOS] GodSpeak Feedback", 
				files: new string[] 
				{
					_fileService.GetFilePath(_messageService.DeliveredMessagesFile),
					_fileService.GetFilePath(_messageService.UpcomingMessagesFile),
					_fileService.GetFilePath("Log.txt")
				});
        }
    }
}

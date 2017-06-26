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

        public FeedbackService (IMessageService messageService, IFileService fileService)
        {
			_messageService = messageService;
			_fileService = fileService;
        }

        public void OpenFeedbackDialog ()
        {
			var deliveredMessages = NSData.FromFile(_fileService.GetFilePath(_messageService.DeliveredMessagesFile));
			var upcomingMessages = NSData.FromFile(_fileService.GetFilePath(_messageService.UpcomingMessagesFile));
			var log = NSData.FromFile(_fileService.GetFilePath("Log.txt"));

			var data = new NSObject[] { deliveredMessages, upcomingMessages, log};
			BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackComposeViewWithPreparedItems(data);
        }
    }
}

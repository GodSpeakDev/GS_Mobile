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

		public FeedbackService(IMessageService messageService, IFileService fileService)
		{
			_messageService = messageService;
			_fileService = fileService;
		}

        public void OpenFeedbackDialog ()
        {
			var deliveredMessages = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath(_messageService.DeliveredMessagesFile)));
			var upcomingMessages = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath(_messageService.UpcomingMessagesFile)));
			var log = Android.Net.Uri.FromFile  (new Java.IO.File(_fileService.GetFilePath("Log.txt")));

			FeedbackManager.ShowFeedbackActivity(Xamarin.Forms.Forms.Context, new Android.Net.Uri[] {deliveredMessages, upcomingMessages, log});
        }
    }
}

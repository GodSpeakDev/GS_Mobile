using System;
using GodSpeak.Services;
using HockeyApp.iOS;
namespace GodSpeak.iOS.Services
{
    public class FeedbackService : IFeedbackService
    {
        public FeedbackService ()
        {
        }

        public void OpenFeedbackDialog ()
        {
            BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackComposeView ();
        }
    }
}

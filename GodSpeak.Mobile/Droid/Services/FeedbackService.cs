using System;
using GodSpeak.Services;
using Android.App;
using HockeyApp.Android;
using Android.Content;

namespace GodSpeak.Droid.Services
{
    public class FeedbackService : IFeedbackService
    {
        readonly Context context;

        public FeedbackService (Context context)
        {
            this.context = context;
        }

        public void OpenFeedbackDialog ()
        {
            FeedbackManager.ShowFeedbackActivity (context);
        }
    }
}

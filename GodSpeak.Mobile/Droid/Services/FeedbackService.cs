using System;
using GodSpeak.Services;
using Android.App;
using HockeyApp.Android;
using Android.Content;

namespace GodSpeak.Droid.Services
{
    public class FeedbackService : IFeedbackService
    {
        public void OpenFeedbackDialog ()
        {
            FeedbackManager.ShowFeedbackActivity (Xamarin.Forms.Forms.Context);
        }
    }
}

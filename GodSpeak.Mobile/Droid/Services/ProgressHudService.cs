using System;
using GodSpeak.Services;
using AndroidHUD;
using Xamarin.Forms;

namespace GodSpeak.Droid.Services
{
    public class ProgressHudService : IProgressHudService
    {
        public ProgressHudService ()
        {
        }

        public void Hide ()
        {
            AndHUD.Shared.Dismiss (Forms.Context);
        }

        public void Show (string message = null)
        {
            AndHUD.Shared.Show (Forms.Context, (message == null) ? "Updating..." : message, -1, MaskType.Black);
        }
    }
}

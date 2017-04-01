using System;
using GodSpeak.Services;
using BigTed;
using UIKit;
namespace GodSpeak.iOS.Services
{
    public class ProgressHudService : IProgressHudService
    {




        public ProgressHudService ()
        {
            ProgressHUD.Shared.HudBackgroundColour = UIColor.FromRGB (0, 165, 255);
            ProgressHUD.Shared.HudForegroundColor = UIColor.White;

        }

        public void Hide ()
        {
            BTProgressHUD.Dismiss ();

        }

        public void Show (string message = null)
        {

            BTProgressHUD.Show ((message == null) ? "Updating..." : message, -1, ProgressHUD.MaskType.Black);



        }
    }
}

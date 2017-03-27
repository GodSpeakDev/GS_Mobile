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


        }

        public void Hide ()
        {
            BTProgressHUD.Dismiss ();
        }

        public void Show ()
        {

            BTProgressHUD.Show (null, -1, ProgressHUD.MaskType.Black);


        }
    }
}

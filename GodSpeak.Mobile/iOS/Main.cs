using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using UIKit;

namespace GodSpeak.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main (string [] args)
        {			
			UISwitch.Appearance.OnTintColor = ColorHelper.Primary.ToUIColor();
			
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main (args, null, "AppDelegate");
        }
    }
}

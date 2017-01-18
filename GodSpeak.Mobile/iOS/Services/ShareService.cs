using System.Threading.Tasks;
using MessageUI;
using UIKit;
using Xamarin.Forms;
using Foundation;
using System.Collections.Generic;

namespace GodSpeak.iOS
{
	public class ShareService : IShareService
	{
		public void Share(string message)
		{
			var activityItems = new List<NSObject>();

			var itemText = NSObject.FromObject(message);
			activityItems.Add(itemText);

			var activityController = new UIActivityViewController(activityItems.ToArray(), null);
			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(activityController, true, null);
		}
	}
}

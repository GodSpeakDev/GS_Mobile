using System;
using GodSpeak;
using UIKit;
using CoreGraphics;

namespace GodSpeak.iOS
{
	public class ImageService : IImageService
	{
		public void Compress(MediaFile mediaFile, float maxHeight, float maxWidth)
		{			
			var sourceImage = UIImage.FromFile(mediaFile.Path);

			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1) 
				return;
			
			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			resultImage.AsJPEG().Save(mediaFile.Path, false);	
		}
	}
}

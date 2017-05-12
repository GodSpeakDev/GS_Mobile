using System;
using GodSpeak;
using Android.Graphics;
using System.IO;

namespace GodSpeak.Droid
{
	public class ImageService : IImageService
	{
		public void Compress(MediaFile mediaFile, float maxHeight, float maxWidth)
		{
			var height = (int)maxHeight;
			var width = (int)maxWidth;

			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(mediaFile.Path, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
								   ? outHeight / height
								   : outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			var resizedBitmap = BitmapFactory.DecodeFile(mediaFile.Path, options);

			if (resizedBitmap != null)
			{
				var stream = new FileStream(mediaFile.Path, FileMode.Create);
				resizedBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
				stream.Close();
			}
		}
	}
}

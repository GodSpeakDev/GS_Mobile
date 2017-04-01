using System;
namespace GodSpeak
{
	public interface IImageService
	{
		void Compress(MediaFile mediaFile, float maxWidth, float maxHeight);
	}
}

using System;
namespace GodSpeak
{
	public class UploadPhotoRequest
	{
		public string Token
		{
			get;
			set;
		}

		public byte[] Photo
		{
			get;
			set;
		}
	}
}

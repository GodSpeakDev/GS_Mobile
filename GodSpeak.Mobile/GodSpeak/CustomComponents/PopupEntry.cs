using System;
using Xamarin.Forms;

namespace GodSpeak
{
	public class PopupEntry : CustomEntry
	{
		public PopupEntry() : base()
		{
			PlaceholderColor = ColorHelper.TextInputPlaceHolder;
			BackgroundColor = ColorHelper.Secondary;
			TextColor = ColorHelper.TextInputFocusedText;
		}
	}
}

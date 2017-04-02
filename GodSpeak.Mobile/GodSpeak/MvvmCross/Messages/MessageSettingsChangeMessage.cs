using System;
using MvvmCross.Plugins.Messenger;

namespace GodSpeak
{
	public class MessageSettingsChangeMessage : MvxMessage
	{
		public MessageSettingsChangeMessage(object sender) : base(sender)
		{
		}
	}
}

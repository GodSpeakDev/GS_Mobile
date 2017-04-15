using System;
using MvvmCross.Plugins.Messenger;

namespace GodSpeak
{
	public class MessageDeliveredMessage : MvxMessage
	{
		public MessageDeliveredMessage(object sender) : base(sender)
		{
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IMessageService
	{
		Task UpdateUpcomingMessages();
		Task<List<Message>> GetDeliveredMessages();
		Task<bool> HasUpcomingMessagesInCache();
		Task<Message> GetSingleMessage(Guid messageId);
	}
}
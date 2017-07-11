using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodSpeak
{
	public interface IMessageService
	{
		string UpcomingMessagesFile
		{
			get;
		}

		string DeliveredMessagesFile
		{
			get;
		}

		Task UpdateUpcomingMessages();
		Task<List<Message>> GetDeliveredMessages();
		Task<bool> HasUpcomingMessagesInCache();
		Task<Message> GetSingleMessage(Guid messageId);
		Task<List<Message>> GetUpcomingMessages();
		Task<bool> HasUpcomingMessagesFile();	
	}
}

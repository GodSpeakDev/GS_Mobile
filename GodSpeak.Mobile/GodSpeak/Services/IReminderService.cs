using System;
namespace GodSpeak
{
	public interface IReminderService
	{
		bool SetMessageReminder(Message message);
		void ClearReminders();
		void AddReminderNotification();
	}
}

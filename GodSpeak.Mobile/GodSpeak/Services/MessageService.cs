﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace GodSpeak
{
    public class MessageService : IMessageService
    {
        private IWebApiService _webApiService;
        private IFileService _fileService;
        private IReminderService _reminderService;
        private ISettingsService _settingsService;
		private ILoggingService _loggingService;

        public string UpcomingMessagesFile {
            get {
                return string.Format ("{0}-upcoming-messages.json", _settingsService.Email);
            }
        }

        public string DeliveredMessagesFile {
            get {
                return string.Format ("{0}-delivered-messages.json", _settingsService.Email);
            }
        }

        public MessageService (IWebApiService webApiService, IFileService fileService, IReminderService reminderService, ISettingsService settingsService, ILogManager logManager)
        {
            _webApiService = webApiService;
            _fileService = fileService;
            _reminderService = reminderService;
            _settingsService = settingsService;
			_loggingService = logManager.GetLog();
        }

        public async Task UpdateUpcomingMessages ()
        {
            var messages = await _webApiService.GetMessages ();

            if (messages.IsSuccess) {
                var messagesToBeReminded = messages.Payload.Where (x => x.DateTimeToDisplay > DateTime.Now).ToList ();
                UpdateReminders (messagesToBeReminded);

                await CacheUpcomingMessages (messages.Payload);
            }
        }

        public async Task<List<Message>> GetDeliveredMessages ()
        {
            var deliveredMessages = await GetAlreadyDeliveredMessages ();
            var upcomingMessages = await GetUpcomingMessages ();

            foreach (var message in upcomingMessages.Where (x => x.DateTimeToDisplay < DateTime.Now.AddMinutes (2) && !deliveredMessages.Any (delivered => delivered.Id == x.Id))) {
                deliveredMessages.Add (message);
				_loggingService.Trace(string.Format("NEW MESSAGE ADDED TO THE DELIVERED FILE: {0}", JsonConvert.SerializeObject(message))); 
            }

            await CacheDeliveredMessages (deliveredMessages);


            return deliveredMessages;
        }

        public async Task<bool> HasUpcomingMessagesInCache ()
        {
            var hasUpcomingFile = await _fileService.ExistsAsync (UpcomingMessagesFile);
			var upcomingMessages = (await GetUpcomingMessages()).Where(x => x.DateTimeToDisplay > DateTime.Now).ToList();

			return hasUpcomingFile && upcomingMessages.Count > 0;
        }

		public async Task<bool> HasUpcomingMessagesFile()
		{
			var hasUpcomingFile = await _fileService.ExistsAsync(UpcomingMessagesFile);
			return hasUpcomingFile;
		}

		public async Task SetReminders()
		{
			var upcomingMessages = (await GetUpcomingMessages()).Where(x => x.DateTimeToDisplay > DateTime.Now.AddMinutes(2));
			
			foreach (var upcomingMessage in upcomingMessages)
			{					
				_reminderService.SetMessageReminder(upcomingMessage);								
			}

			_reminderService.AddReminderNotification();
		}

        public async Task<Message> GetSingleMessage (Guid messageId)
        {
            var deliveredMessages = await GetDeliveredMessages ();
            var message = deliveredMessages.FirstOrDefault (x => x.Id == messageId);

            if (message == null) {
                var upcomingMessages = await GetUpcomingMessages ();
                message = upcomingMessages.FirstOrDefault (x => x.Id == messageId);
            }

            return message;
        }

        private async Task CacheUpcomingMessages (List<Message> messages)
        {
            var fileExists = await _fileService.ExistsAsync (UpcomingMessagesFile);
            if (fileExists) {
                await _fileService.DeleteFileAsync (UpcomingMessagesFile);
            }

            await _fileService.WriteTextAsync (UpcomingMessagesFile, Newtonsoft.Json.JsonConvert.SerializeObject (messages));
        }

        private void UpdateReminders (List<Message> messages)
        {
            _reminderService.ClearReminders ();
			foreach (var message in messages.OrderBy(x => x.DateTimeToDisplay)) 
			{
                _reminderService.SetMessageReminder (message);
            }

			_reminderService.AddReminderNotification();
        }

        public async Task<List<Message>> GetUpcomingMessages ()
        {
            var fileExists = await _fileService.ExistsAsync (UpcomingMessagesFile);
            if (!fileExists) {
                return new List<Message> ();
            } else {
                var fileContent = await _fileService.ReadTextAsync (UpcomingMessagesFile);
                return JsonConvert.DeserializeObject<List<Message>> (fileContent);
            }
        }

        private async Task<List<Message>> GetAlreadyDeliveredMessages ()
        {
            var fileExists = await _fileService.ExistsAsync (DeliveredMessagesFile);
            if (!fileExists) {
                return new List<Message> ();
            } else {
                var fileContent = await _fileService.ReadTextAsync (DeliveredMessagesFile);
                return JsonConvert.DeserializeObject<List<Message>> (fileContent);
            }
        }

        private async Task CacheDeliveredMessages (List<Message> messages)
        {
            var fileExists = await _fileService.ExistsAsync (DeliveredMessagesFile);
            if (fileExists) {
                await _fileService.DeleteFileAsync (DeliveredMessagesFile);
            }

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(messages);
            await _fileService.WriteTextAsync (DeliveredMessagesFile, json);
			_loggingService.Trace(string.Format("DELIVERED FILE: {0}", json)); 
        }
    }
}

using System;
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

        public MessageService (IWebApiService webApiService, IFileService fileService, IReminderService reminderService, ISettingsService settingsService)
        {
            _webApiService = webApiService;
            _fileService = fileService;
            _reminderService = reminderService;
            _settingsService = settingsService;
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
            }

            await CacheDeliveredMessages (deliveredMessages);
            return deliveredMessages;
        }

        public async Task<bool> HasUpcomingMessagesInCache ()
        {
            return await _fileService.ExistsAsync (UpcomingMessagesFile);
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
            foreach (var message in messages) {
                _reminderService.SetMessageReminder (message);
            }
        }

        private async Task<List<Message>> GetUpcomingMessages ()
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

            await _fileService.WriteTextAsync (DeliveredMessagesFile, Newtonsoft.Json.JsonConvert.SerializeObject (messages));
        }
    }
}

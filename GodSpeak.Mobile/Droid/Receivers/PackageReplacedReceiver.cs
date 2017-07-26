using System;
using Android.App;
using Android.Content;
using GodSpeak.Api;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace GodSpeak.Droid.Receivers
{
	[BroadcastReceiver(Exported = true)]
    [IntentFilter(new[] { Intent.ActionMyPackageReplaced })]
	public class PackageReplacedReceiver : Android.Support.V4.Content.WakefulBroadcastReceiver
	{
		public async override void OnReceive(Context context, Intent intent)
		{
            var logManager = new NLogManager();
            var logger = logManager.GetLog();
			logger.Info("PACKAGE REPLACED RECEIVER");            			

            try
            {
				var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
				setup.EnsureInitialized();

                var settingsService = Mvx.Resolve<ISettingsService>();
                var messageService = Mvx.Resolve<IMessageService>();

                if (settingsService.Token != null)
                {
                    //var messageService = new MessageService(azureWebApiService, fileService, reminderService, settingsService, logManager);
                    logger.Info("Message Service OK: " + (messageService != null).ToString());

                    ((messageService as MessageService).ReminderService as ReminderService).Context = context;
                    await messageService.SetReminders();
                }
                else
                {
                   logger.Info("Settings.Token is null"); 
                }
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(ex));
            }
        }
	}
}

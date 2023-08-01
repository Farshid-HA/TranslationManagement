using External.ThirdParty.Services;
using System;
using TranslationManagement.Domain.IServices;

namespace TranslationManagement.Services.Services
{
    public class ReliableNotificationService : IReliableNotificationService
    {
        private readonly INotificationService notificationService;

        public ReliableNotificationService(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        static int maxRetries = 1000;
        public async void SendNotification(string text, int currentRetry = 1)
        {
            currentRetry++;
            try
            {
                while (!await notificationService.SendNotification(text) && currentRetry <= maxRetries)
                {
                    currentRetry++;
                }
            }
            catch
            {
                //if we have an error, we will try to run it again.
                if (currentRetry <= maxRetries)
                {
                    SendNotification(text, currentRetry);
                }
            }
        }
    }
}

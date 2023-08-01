namespace TranslationManagement.Domain.IServices
{
    public interface IReliableNotificationService
    {
        void SendNotification(string text, int currentRetry = 1);
    }
}

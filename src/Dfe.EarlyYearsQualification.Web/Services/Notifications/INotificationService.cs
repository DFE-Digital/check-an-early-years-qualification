namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public interface INotificationService
{
    void SendFeedbackNotification(FeedbackNotification feedbackNotification);
}
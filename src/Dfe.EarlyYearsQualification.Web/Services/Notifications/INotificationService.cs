namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public interface INotificationService
{
    void SendHelpPageNotification(HelpPageNotification helpPageNotification);
    
    void SendEmbeddedFeedbackFormNotification(EmbeddedFeedbackFormNotification  embeddedFeedbackFormNotification);
}
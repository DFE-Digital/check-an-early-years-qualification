using Microsoft.Extensions.Options;
using Notify.Client;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class GovUkNotifyService(IOptions<NotificationOptions> notificationOptions) : INotificationService
{
    public void SendFeedbackNotification(FeedbackNotification feedbackNotification)
    {
        var options = notificationOptions.Value;
        var client = new NotificationClient(options.ApiKey);
        var personalisation = new Dictionary<string, dynamic>
                              {
                                  { "subject", feedbackNotification.Subject },
                                  { "email_address", feedbackNotification.EmailAddress },
                                  { "message", feedbackNotification.Message }
                              };
        
        client.SendEmail(options.Feedback.EmailAddress, options.Feedback.TemplateId, personalisation);
    }
}
using Microsoft.Extensions.Options;
using Notify.Exceptions;
using Notify.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class GovUkNotifyService(
    ILogger<GovUkNotifyService> logger,
    IOptions<NotificationOptions> notificationOptions, 
    INotificationClient client) : INotificationService
{
    public void SendFeedbackNotification(FeedbackNotification feedbackNotification)
    {
        try
        {
            var options = notificationOptions.Value;
            var personalisation = new Dictionary<string, dynamic>
                                  {
                                      { "subject", feedbackNotification.Subject },
                                      { "email_address", feedbackNotification.EmailAddress },
                                      { "message", feedbackNotification.Message }
                                  };
        
            client.SendEmail(options.Feedback.EmailAddress, options.Feedback.TemplateId, personalisation);
        }
        catch (NotifyClientException exception)
        {
            logger.LogError("Error thrown from GovUKNotifyService: {Message}", exception.Message);
        }
    }
}
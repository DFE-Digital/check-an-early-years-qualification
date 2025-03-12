using Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;
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
            var subjectPrefix = options.IsTestEnvironment ? "TEST - " : string.Empty;
            var personalisation = new Dictionary<string, dynamic>
                                  {
                                      { "subject", $"{subjectPrefix}{feedbackNotification.Subject}" },
                                      { "selected_option", feedbackNotification.Subject },
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
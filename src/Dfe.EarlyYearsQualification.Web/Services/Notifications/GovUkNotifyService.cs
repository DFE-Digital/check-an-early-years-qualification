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
    public void SendHelpPageNotification(HelpPageNotification helpPageNotification)
    {
        try
        {
            var options = notificationOptions.Value;
            var subjectPrefix = options.IsTestEnvironment ? "TEST - " : string.Empty;
            var emailAddress = helpPageNotification.EmailAddress ?? "Not supplied";
            var personalisation = new Dictionary<string, dynamic>
                                  {
                                      { "subject", $"{subjectPrefix}{helpPageNotification.Subject}" },
                                      { "selected_option", helpPageNotification.Subject },
                                      { "email_address", emailAddress },
                                      { "message", helpPageNotification.Message }
                                  };
        
            client.SendEmail(options.HelpPageForm.EmailAddress, options.HelpPageForm.TemplateId, personalisation);
        }
        catch (NotifyClientException exception)
        {
            logger.LogError("Error thrown from GovUKNotifyService: {Message}", exception.Message);
        }
    }

    public void SendEmbeddedFeedbackFormNotification(EmbeddedFeedbackFormNotification embeddedFeedbackFormNotification)
    {
        try
        {
            var options = notificationOptions.Value;
            var subjectPrefix = options.IsTestEnvironment ? "TEST - " : string.Empty;
            var personalisation = new Dictionary<string, dynamic>
                                  {
                                      { "subject", $"{subjectPrefix}Feedback form submission" },
                                      { "message", embeddedFeedbackFormNotification.Message }
                                  };
        
            client.SendEmail(options.EmbeddedFeedbackForm.EmailAddress, options.EmbeddedFeedbackForm.TemplateId, personalisation);
        }
        catch (NotifyClientException exception)
        {
            logger.LogError("Error thrown from GovUKNotifyService: {Message}", exception.Message);
        }
    }
}
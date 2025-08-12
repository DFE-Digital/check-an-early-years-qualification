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
        var emailAddress = helpPageNotification.EmailAddress ?? "Not supplied";
        var personalisation = new Dictionary<string, dynamic>
                              {
                                  { "selected_option", helpPageNotification.Subject },
                                  { "email_address", emailAddress },
                                  { "message", helpPageNotification.Message }
                              };
        SendEmail(notificationOptions.Value.HelpPageForm, personalisation, helpPageNotification.Subject);
    }

    public void SendEmbeddedFeedbackFormNotification(EmbeddedFeedbackFormNotification embeddedFeedbackFormNotification)
    {
        var personalisation = new Dictionary<string, dynamic>
        {
            { "message", embeddedFeedbackFormNotification.Message }
        };
        SendEmail(notificationOptions.Value.EmbeddedFeedbackForm, personalisation, "Feedback form submission");
    }

    private void SendEmail(NotificationData notificationData, Dictionary<string, dynamic> personalisation, string subject)
    {
        try
        {
            var options = notificationOptions.Value;
            var subjectPrefix = options.IsTestEnvironment ? "TEST - " : string.Empty;
            personalisation.Add("subject", $"{subjectPrefix}{subject}");
        
            client.SendEmail(notificationData.EmailAddress, notificationData.TemplateId, personalisation);
        }
        catch (NotifyClientException exception)
        {
            logger.LogError("Error thrown from GovUKNotifyService: {Message}", exception.Message);
        }
    }
}
namespace Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;

public class NotificationOptions
{
    public string ApiKey { get; init; } = string.Empty;
    
    public bool IsTestEnvironment { get; init; }

    public NotificationData HelpPageForm { get; init; } = new NotificationData();
    
    public NotificationData EmbeddedFeedbackForm { get; init; } = new NotificationData();
}
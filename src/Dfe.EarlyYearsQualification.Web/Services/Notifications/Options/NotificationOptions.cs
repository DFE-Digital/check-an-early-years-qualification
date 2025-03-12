namespace Dfe.EarlyYearsQualification.Web.Services.Notifications.Options;

public class NotificationOptions
{
    public string ApiKey { get; init; } = string.Empty;

    public Feedback Feedback { get; init; } = new();
}
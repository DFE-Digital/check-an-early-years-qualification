namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class NotificationOptions
{
    public string ApiKey { get; init; } = string.Empty;

    public Feedback Feedback { get; init; } = new();
}

public class Feedback
{
    public string TemplateId { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;
}
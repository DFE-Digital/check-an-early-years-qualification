namespace Dfe.EarlyYearsQualification.Web.Services.Notifications;

public class FeedbackNotification
{
    public string Subject { get; init; } = string.Empty;

    public string? EmailAddress { get; init; }

    public string Message { get; init; } = string.Empty;
}
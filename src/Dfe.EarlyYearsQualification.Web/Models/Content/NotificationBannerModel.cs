namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class NotificationBannerModel
{
    public string Id { get; init; } = "notification-banner";

    public string Heading { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;

    public bool FullWidth { get; init; }
}

using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class NotificationBanner
{
    public string Heading { get; init; } = string.Empty;

    public Document Body { get; init; } = new Document();
}

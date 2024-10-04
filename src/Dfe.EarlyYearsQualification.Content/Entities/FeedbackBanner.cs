using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackBanner
{
    public string Heading { get; init; } = string.Empty;

    public Document? Body { get; init; }
    
    public string BannerTitle { get; init; } = string.Empty;
}
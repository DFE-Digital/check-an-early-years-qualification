using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackFormPage
{
    public required string Heading { get; set; }

    public Document? PostHeadingContent { get; set; }

    public List<IFeedbackFormQuestion> Questions { get; set; } = [];

    public required NavigationLink BackButton { get; set; }

    public required string CtaButtonText { get; set; }

    public required string ErrorBannerHeading { get; set; }
}
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class HelpConfirmationPage
{
    public string SuccessMessage { get; init; } = string.Empty;
    
    public string BodyHeading { get; init; } = string.Empty;

    public Document? Body { get; init; }

    public FeedbackComponent? FeedbackComponent { get; init; }

    public NavigationLink? ReturnToHomepageLink { get; init; }
}
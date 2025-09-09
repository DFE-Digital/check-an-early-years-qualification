namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class ConfirmationPageViewModel
{
    // Contentful fields
    public string SuccessMessage { get; init; } = string.Empty;

    public string SuccessMessageFollowingText { get; init; } = string.Empty;

    public string BodyHeading { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;
    
    public FeedbackComponentModel? FeedbackComponent { get; init; }

    public NavigationLinkModel? ReturnToTheHomepageLink { get; init; }
}
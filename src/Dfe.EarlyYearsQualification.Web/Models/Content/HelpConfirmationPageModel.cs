namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class HelpConfirmationPageModel
{
    public string SuccessMessage { get; init; } = string.Empty;

    public string BodyHeading { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;
    
    public FeedbackComponentModel? FeedbackComponent { get; init; }

    public NavigationLinkModel? ReturnToTheHomepageLink { get; init; }
}
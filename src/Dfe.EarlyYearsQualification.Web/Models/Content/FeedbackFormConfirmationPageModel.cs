namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class FeedbackFormConfirmationPageModel
{
    public string SuccessMessage { get; init; } = string.Empty;
    
    public string Body { get; init; } = string.Empty;

    public bool ShowOptionalSection { get; set; }

    public string OptionalEmailHeading { get; set; } = string.Empty;
    
    public string OptionalEmailBody { get; set; } = string.Empty;

    public NavigationLinkModel? ReturnToHomepageLink { get; init; }
}
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class FeedbackFormConfirmationPage
{
    public string SuccessMessage { get; init; } = string.Empty;
    
    public Document? Body { get; init; }

    public string OptionalEmailHeading { get; set; } = string.Empty;
    
    public Document? OptionalEmailBody { get; set; }

    public NavigationLink? ReturnToHomepageLink { get; init; }
}
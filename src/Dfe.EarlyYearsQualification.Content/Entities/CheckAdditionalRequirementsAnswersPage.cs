using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CheckAdditionalRequirementsAnswerPage
{
    public string PageHeading { get; init; } = string.Empty;

    public string AnswerDisclaimerText { get; init; } = string.Empty;
    
    public string ChangeAnswerText { get; init; } = string.Empty;

    public string ButtonText { get; init; } = string.Empty;
    
    public NavigationLink? BackButton { get; init; }
}
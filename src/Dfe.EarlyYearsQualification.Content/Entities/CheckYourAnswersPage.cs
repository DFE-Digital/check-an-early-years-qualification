namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CheckYourAnswersPage
{
    public string PageHeading { get; init; } = string.Empty;
    
    public string ChangeAnswerText { get; init; } = string.Empty;
    
    public string QualificationStartedText { get; init; } = string.Empty;
    
    public string QualificationAwardedText { get; init; } = string.Empty;
    
    public string AnyAwardingOrganisationText { get; init; } = string.Empty;
    
    public string AnyLevelText { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;
    
    public NavigationLink? BackButton { get; init; }
}
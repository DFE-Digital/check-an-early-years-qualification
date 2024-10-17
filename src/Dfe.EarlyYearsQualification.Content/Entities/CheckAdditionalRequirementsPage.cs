namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CheckAdditionalRequirementsPage
{
    public string Heading { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }
    
    public NavigationLink? PreviousQuestionBackButton { get; init; }

    public string QualificationLabel { get; init; } = string.Empty;

    public string QualificationLevelLabel { get; init; } = string.Empty;

    public string AwardingOrganisationLabel { get; init; } = string.Empty;

    public string InformationMessage { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public string QuestionSectionHeading { get; init; } = string.Empty;

    public string ErrorMessage { get; init; } = string.Empty;

    public string ErrorSummaryHeading { get; init; } = string.Empty;
}
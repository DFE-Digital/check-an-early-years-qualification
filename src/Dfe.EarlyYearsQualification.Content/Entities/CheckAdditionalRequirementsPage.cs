namespace Dfe.EarlyYearsQualification.Content.Entities;

public class CheckAdditionalRequirementsPage
{
    public string Heading { get; set; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public string QualificationLabel { get; set; } = string.Empty;

    public string QualificationLevelLabel { get; set; } = string.Empty;

    public string AwardingOrganisationLabel { get; set; } = string.Empty;

    public string InformationMessage { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;
}
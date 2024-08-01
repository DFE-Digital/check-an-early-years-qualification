namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Qualification(
    string qualificationId,
    string qualificationName,
    string awardingOrganisationTitle,
    int qualificationLevel)
{
    // Required Fields
    public string QualificationId { get; } = qualificationId;
    public string QualificationName { get; } = qualificationName;
    public string AwardingOrganisationTitle { get; } = awardingOrganisationTitle;
    public int QualificationLevel { get; } = qualificationLevel;

    // Optional Fields
    public string? FromWhichYear { get; init; }
    public string? ToWhichYear { get; init; }
    public string? QualificationNumber { get; init; }
    public string? AdditionalRequirements { get; init; }
    public List<AdditionalRequirementQuestion>? AdditionalRequirementQuestions { get; init; }
    public List<RatioRequirement>? RatioRequirements { get; init; }
}
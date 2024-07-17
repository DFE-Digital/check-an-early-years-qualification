namespace Dfe.EarlyYearsQualification.ContentUpload;

public class QualificationUpload(
    string qualificationId,
    string qualificationName,
    string awardingOrganisationTitle,
    int qualificationLevel,
    string? fromWhichYear,
    string? toWhichYear,
    string? qualificationNumber,
    string? additionalRequirements,
    string[]? additionalRequirementQuestions,
    string[]? ratioRequirements)
{
    // Required Fields
    public string QualificationId { get; } = qualificationId;
    public string QualificationName { get; } = qualificationName;
    public string AwardingOrganisationTitle { get; } = awardingOrganisationTitle;
    public int QualificationLevel { get; } = qualificationLevel;

    // Optional Fields
    public string? FromWhichYear { get; } = fromWhichYear;
    public string? ToWhichYear { get; } = toWhichYear;
    public string? QualificationNumber { get; } = qualificationNumber;
    public string? AdditionalRequirements { get; } = additionalRequirements;

    public string[]? AdditionalRequirementQuestions { get; } = additionalRequirementQuestions;

    public string[]? RatioRequirements { get; } = ratioRequirements;
}
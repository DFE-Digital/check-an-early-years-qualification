namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Qualification
{
    public Qualification(
        string qualificationId,
        string qualificationName,
        string awardingOrganisationTitle,
        int qualificationLevel,
        string? fromWhichYear,
        string? toWhichYear,
        string? qualificationNumber,
        string? additionalRequirements,
        List<AdditionalRequirementQuestion>? additionalRequirementQuestions,
        List<RatioRequirement>? ratioRequirements)
    {
        QualificationId = qualificationId;
        QualificationName = qualificationName;
        AwardingOrganisationTitle = awardingOrganisationTitle;
        QualificationLevel = qualificationLevel;
        FromWhichYear = fromWhichYear;
        ToWhichYear = toWhichYear;
        QualificationNumber = qualificationNumber;
        AdditionalRequirements = additionalRequirements;
        AdditionalRequirementQuestions = additionalRequirementQuestions;
        RatioRequirements = ratioRequirements;
    }

    // Required Fields
    public string QualificationId { get; }
    public string QualificationName { get; }
    public string AwardingOrganisationTitle { get; }
    public int QualificationLevel { get; }

    // Optional Fields
    public string? FromWhichYear { get; }
    public string? ToWhichYear { get; }
    public string? QualificationNumber { get; }
    public string? AdditionalRequirements { get; }
    public List<AdditionalRequirementQuestion>? AdditionalRequirementQuestions { get; }
    public List<RatioRequirement>? RatioRequirements { get; }
}
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
        string? additionalRequirements)
    {
        QualificationId = qualificationId;
        QualificationName = qualificationName;
        AwardingOrganisationTitle = awardingOrganisationTitle;
        QualificationLevel = qualificationLevel;
        FromWhichYear = fromWhichYear;
        ToWhichYear = toWhichYear;
        QualificationNumber = qualificationNumber;
        AdditionalRequirements = additionalRequirements;
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
}
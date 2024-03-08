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
        string? notes,
        string? additionalRequirements)
    {
        QualificationId = qualificationId;
        QualificationName = qualificationName;
        AwardingOrganisationTitle = awardingOrganisationTitle;
        QualificationLevel = qualificationLevel;
        FromWhichYear = fromWhichYear;
        ToWhichYear = toWhichYear;
        QualificationNumber = qualificationNumber;
        Notes = notes;
        AdditionalRequirements = additionalRequirements;
    }
    // Required Fields
    public string QualificationId { get; set; }
    public string QualificationName { get; set; }
    public string AwardingOrganisationTitle { get; set; }
    public int QualificationLevel { get; set; }

    // Optional Fields
    public string? FromWhichYear { get; set; }
    public string? ToWhichYear { get; set; }
    public string? QualificationNumber { get; set; }
    public string? Notes { get; set; }
    public string? AdditionalRequirements { get; set; }
}
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Qualification(
  int qualificationId,
  string qualificationName,
  string awardingOrganisationTitle,
  string qualificaitonLevel,
  string? fromWhichYear,
  string? qualificationNumber,
  string? notesAdditionalRequirements)
{
  // Required Fields
  public int QualificationId { get; set; } = qualificationId;
  public string QualificationName { get; set; } = qualificationName;
  public string AwardingOrganisationTitle { get; set; } = awardingOrganisationTitle;
  public string QualificationLevel { get; set; } = qualificaitonLevel;

  // Optional Fields
  public string? FromWhichYear { get; set; } = fromWhichYear;
  public string? QualificationNumber { get; set; } = qualificationNumber;
  public string? NotesAdditionalRequirements { get; set; } = notesAdditionalRequirements;
}
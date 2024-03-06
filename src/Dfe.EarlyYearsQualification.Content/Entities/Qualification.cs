using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class Qualification(
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
  // Required Fields
  public string QualificationId { get; set; } = qualificationId;
  public string QualificationName { get; set; } = qualificationName;
  public string AwardingOrganisationTitle { get; set; } = awardingOrganisationTitle;
  public int QualificationLevel { get; set; } = qualificationLevel;

  // Optional Fields
  public string? FromWhichYear { get; set; } = fromWhichYear;
  public string? ToWhichYear { get; set; } = toWhichYear;
  public string? QualificationNumber { get; set; } = qualificationNumber;
  public string? Notes { get; set; } = notes;
  public string? AdditionalRequirements { get; set; } = additionalRequirements;
}
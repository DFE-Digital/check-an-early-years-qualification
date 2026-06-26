using Contentful.Core.Models;

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
    public int StaffChildRatio { get; init; }
    public List<Tab> EyqlTabs { get; init; } = [];

    // Optional Fields
    public string? FromWhichYear { get; init; }
    public string? ToWhichYear { get; init; }
    public string? QualificationNumber { get; init; }
    public List<AdditionalRequirementQuestion>? AdditionalRequirementQuestions { get; init; }
    public List<RatioRequirement>? RatioRequirements { get; set; }
    public bool IsAutomaticallyApprovedAtLevel6 { get; init; }
    public bool IsTheQualificationADegree { get; set; }
    public Document? AdditionalRequirementsRichText { get; init; }
    public string? AdditionalRequirementsPlainText { get; init; }
    public string? Notes { get; init; }
}
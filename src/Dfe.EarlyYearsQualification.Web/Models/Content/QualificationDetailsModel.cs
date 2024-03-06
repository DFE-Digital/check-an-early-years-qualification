namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel
{
    public string QualificationId { get; set; } = string.Empty;
    public string QualificationName { get; set; } = string.Empty;
    public string AwardingOrganisationTitle { get; set; } = string.Empty;
    public string QualificationLevel { get; set; } = string.Empty;

    public string? FromWhichYear { get; set; }
    public string? ToWhichYear { get; set; }
    public string? QualificationNumber { get; set; }
    public string? Notes { get; set; }
    public string? AdditionalRequirements { get; set; }
}
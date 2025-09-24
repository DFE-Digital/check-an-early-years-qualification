namespace Dfe.EarlyYearsQualification.Web.Models.List;

public class QualificationModel
{
    public string QualificationId { get; set; } = string.Empty;

    public string QualificationName { get; init; } = string.Empty;

    public string AwardingOrganisationTitle { get; init; } = string.Empty;

    public int QualificationLevel { get; init; }

    public string FromWhichYear { get; init; } = string.Empty;

    public string? QualificationNumber { get; init; } = string.Empty;

    public string? AdditionalRequirements { get; init; } = string.Empty;
}
namespace Dfe.EarlyYearsQualification.Web.Models.List;

public class QualificationModel
{
    public string QualificationId { get; set; } = string.Empty;

    public string QualificationName { get; set; } = string.Empty;

    public string AwardingOrganisationTitle { get; set; } = string.Empty;

    public int QualificationLevel { get; set; }

    public string FromWhichYear { get; set; } = string.Empty;

    public string? QualificationNumber { get; set; } = string.Empty;
}
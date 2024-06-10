namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationCardModel
{
    public string QualificationId { get; init; } = string.Empty;
    public string QualificationName { get; init; } = string.Empty;
    public string AwardingOrganisationTitle { get; init; } = string.Empty;
    public int QualificationLevel { get; init; }
}
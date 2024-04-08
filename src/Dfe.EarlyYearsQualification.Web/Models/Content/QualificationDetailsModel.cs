using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel
{
    public string QualificationId { get; set; } = string.Empty;
    public string QualificationName { get; set; } = string.Empty;
    public string AwardingOrganisationTitle { get; set; } = string.Empty;
    public int QualificationLevel { get; set; }

    public string? FromWhichYear { get; set; }
    public string? ToWhichYear { get; set; }
    public string? QualificationNumber { get; set; }
    public string? Notes { get; set; }
    public string? AdditionalRequirements { get; set; }

    public string BookmarkUrl {get; set;} = string.Empty;

    public DetailsPage? Content { get; set; }
}
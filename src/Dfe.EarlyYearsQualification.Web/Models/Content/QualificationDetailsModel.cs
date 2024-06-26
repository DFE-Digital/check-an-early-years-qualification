namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel
{
    public string QualificationId { get; init; } = string.Empty;
    public string QualificationName { get; init; } = string.Empty;
    public string AwardingOrganisationTitle { get; init; } = string.Empty;
    public int QualificationLevel { get; init; }

    public string? FromWhichYear { get; init; }
    public string? ToWhichYear { get; init; }
    public string? QualificationNumber { get; init; }
    public string? AdditionalRequirements { get; init; }

    public string BookmarkUrl { get; init; } = string.Empty;

    public DetailsPageModel? Content { get; init; }
}
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationDetailsModel : BasicQualificationModel
{
    public string? FromWhichYear { get; init; }
    public string? ToWhichYear { get; init; }
    public string? QualificationNumber { get; init; }
    public string? AdditionalRequirements { get; init; }

    public string BookmarkUrl { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public DetailsPageModel? Content { get; init; }
}
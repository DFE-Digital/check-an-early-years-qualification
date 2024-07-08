using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationListModel
{
    public string Header { get; init; } = string.Empty;

    public FilterModel Filters { get; init; } = new();

    public NavigationLink? BackButton { get; init; }
    
    public string SingleQualificationFoundText { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public string PreSearchBoxContent { get; init; } = string.Empty;

    public string SearchButtonText { get; init; } = string.Empty;

    public string LevelHeading { get; init; } = string.Empty;

    public string AwardingOrganisationHeading { get; init; } = string.Empty;

    public string PostQualificationListContent { get; init; } = string.Empty;

    public string SearchCriteriaHeading { get; init; } = string.Empty;

    public string PostSearchCriteriaContent { get; init; } = string.Empty;

    public string? SearchCriteria { get; set; } = string.Empty;

    public List<BasicQualificationModel> Qualifications { get; init; } = [];
}
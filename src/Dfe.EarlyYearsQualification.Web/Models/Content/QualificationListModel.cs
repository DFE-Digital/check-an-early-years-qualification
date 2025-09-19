namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class QualificationListModel
{
    public string Header { get; init; } = string.Empty;

    public FilterModel Filters { get; init; } = new();

    public NavigationLinkModel? BackButton { get; init; }

    public string QualificationFoundPrefixText { get; init; } = string.Empty;

    public string SingleQualificationFoundText { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public string PreSearchBoxContent { get; init; } = string.Empty;

    public string SearchButtonText { get; init; } = string.Empty;

    public string PostQualificationListContent { get; init; } = string.Empty;

    public string SearchCriteriaHeading { get; init; } = string.Empty;

    public string? SearchCriteria { get; init; } = string.Empty;

    public List<BasicQualificationModel> Qualifications { get; init; } = [];

    public string NoResultText { get; init; } = string.Empty;

    public string ClearSearchText { get; init; } = string.Empty;

    public string QualificationNumberLabel { get; init; } = string.Empty;
}
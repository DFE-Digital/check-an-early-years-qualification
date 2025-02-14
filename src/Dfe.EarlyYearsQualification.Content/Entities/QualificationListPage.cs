using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QualificationListPage
{
    public string Header { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public string SingleQualificationFoundText { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public Document? PreSearchBoxContent { get; init; }

    public string SearchButtonText { get; init; } = string.Empty;

    public string LevelHeading { get; init; } = string.Empty;

    public string AwardingOrganisationHeading { get; init; } = string.Empty;

    public Document? PostQualificationListContent { get; init; }

    public string SearchCriteriaHeading { get; init; } = string.Empty;

    public Document? PostSearchCriteriaContent { get; init; }

    public string AnyLevelHeading { get; init; } = string.Empty;

    public string AnyAwardingOrganisationHeading { get; init; } = string.Empty;
    
    public Document? NoResultsText { get; init; }

    public string ClearSearchText { get; init; } = string.Empty;

    public string NoQualificationsFoundText { get; init; } = string.Empty;

    public string StartDatePrefixText { get; set; } = string.Empty;
    public string AwardedDatePrefixText { get; set; } = string.Empty;
}
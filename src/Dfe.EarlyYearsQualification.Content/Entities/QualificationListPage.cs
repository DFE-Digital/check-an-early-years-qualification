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
}
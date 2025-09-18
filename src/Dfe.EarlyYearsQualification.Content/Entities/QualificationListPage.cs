using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QualificationListPage
{
    public string Header { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public string QualificationFoundPrefix { get; init; } = string.Empty;

    public string SingleQualificationFoundText { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public Document? PreSearchBoxContent { get; init; }

    public string SearchButtonText { get; init; } = string.Empty;

    public string Pre2014L6OrNotSureContentHeading { get; init; } = string.Empty;
    
    public Document? Pre2014L6OrNotSureContent { get; init; }
    
    public string Post2014L6OrNotSureContentHeading { get; init; } = string.Empty;
    
    public Document? Post2014L6OrNotSureContent { get; init; }

    public string PostQualificationListContentHeading { get; init; } = string.Empty;
    
    public Document? PostQualificationListContent { get; init; }

    public string SearchCriteriaHeading { get; init; } = string.Empty;

    public string AnyLevelHeading { get; init; } = string.Empty;

    public string AnyAwardingOrganisationHeading { get; init; } = string.Empty;

    public Document? NoResultsText { get; init; }

    public string ClearSearchText { get; init; } = string.Empty;

    public string AwardedLocationPrefixText { get; init; } = string.Empty;

    public string StartDatePrefixText { get; init; } = string.Empty;

    public string AwardedDatePrefixText { get; init; } = string.Empty;

    public string LevelPrefixText { get; init; } = string.Empty;

    public string AwardedByPrefixText { get; init; } = string.Empty;
}
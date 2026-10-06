using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class QualificationListPage
{
    public string Header { get; init; } = string.Empty;

    public NavigationLink? BackButton { get; init; }

    public string QualificationFoundPrefix { get; init; } = string.Empty;

    public string SingleQualificationFoundText { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public string SearchButtonText { get; init; } = string.Empty;

    public string PostQualificationListContentHeading { get; init; } = string.Empty;
    
    public Document? PostQualificationListContent { get; init; }

    public string SearchCriteriaHeading { get; init; } = string.Empty;

    public string AnyLevelHeading { get; init; } = string.Empty;

    public string AnyAwardingOrganisationHeading { get; init; } = string.Empty;

    public Document? NoResultsText { get; init; }

    public string NoMatchingQualificationsHeading { get; init; } = string.Empty;

    public string ClearSearchText { get; init; } = string.Empty;

    public string AwardedLocationPrefixText { get; init; } = string.Empty;

    public string StartDatePrefixText { get; init; } = string.Empty;

    public string StartDateBeforeSept2014PrefixText { get; init; } = string.Empty;

    public string AwardedDatePrefixText { get; init; } = string.Empty;

    public string LevelPrefixText { get; init; } = string.Empty;

    public string AwardedByPrefixText { get; init; } = string.Empty;

    public string QualificationNumberLabel { get; init; } = string.Empty;

    public string SearchWithinSingleHeading { get; init; } = string.Empty;

    public string SearchWithinMultipleHeadingFormat { get; init; } = string.Empty;

    public string EnterKeywordsSingleContent { get; init; } = string.Empty;

    public string EnterKeywordsMultipleContentFormat { get; init; } = string.Empty;

    public string SearchMatchHeadingFormat { get; init; } = string.Empty;

    public string SearchNoMatchGuidanceIntroFormat { get; init; } = string.Empty;

    public Document? SearchNoMatchGuidance { get; init; }

    public List<SearchResultContent>? SearchResultsContent { get; init; }
}
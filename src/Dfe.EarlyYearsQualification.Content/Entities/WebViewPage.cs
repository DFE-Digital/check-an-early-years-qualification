using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class WebViewPage
{
    public NavigationLink? BackButton { get; init; }

    public string Heading  { get; init; } = string.Empty;

    public Document PostHeadingContent  { get; init; } = new();

    public string DownloadButtonText  { get; init; } = string.Empty;

    public string QualificationLevelLabel  { get; init; } = string.Empty;

    public string StaffChildRatioLabel  { get; init; } = string.Empty;

    public string FromWhichYearLabel { get; init; } = string.Empty;

    public string ToWhichYearLabel { get; init; } = string.Empty;

    public string AwardingOrganisationLabel  { get; init; } = string.Empty;

    public string QualificationNumberLabel  { get; init; } = string.Empty;

    public string NotesAdditionalRequirementsLabel  { get; init; } = string.Empty;

    public string ShowingAllQualificationsLabel { get; init; } = string.Empty;

    public Document NoQualificationsFoundContent { get; init; } = new();

    public Document QualificationIsFullAndRelevantContent  { get; init; } = new();

    public string FilterHeading  { get; init; } = string.Empty;

    public string SelectedFiltersHeading  { get; init; } = string.Empty;

    public string KeywordHeading  { get; init; } = string.Empty;

    public string QualificationStartDateHeading  { get; init; } = string.Empty;

    public string QualificationLevelHeading  { get; init; } = string.Empty;

    public string ApplyFiltersButtonContent  { get; init; } = string.Empty;

    public string ClearFiltersLinkLabel { get; init; } = string.Empty;

    public string NoFiltersSelectedContent  { get; init; } = string.Empty;

    public List<IOptionItem> StartDateFilters { get; init; } = [];

    public List<IOptionItem> LevelFilters { get; init; } = [];

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public string SingleQualificationFoundText { get; init; } = string.Empty;
}
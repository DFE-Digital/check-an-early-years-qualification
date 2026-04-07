using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class EarlyYearsQualificationListModel
{
    public NavigationLinkModel? BackButton { get; init; }

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;

    public string DownloadButtonText { get; init; } =  string.Empty;

    public string QualificationLevelLabel { get; init; } = string.Empty;

    public string StaffChildRatioLabel { get; init; } = string.Empty;

    public string FromWhichYearLabel { get; init; } = string.Empty;

    public string ToWhichYearLabel { get; init; } = string.Empty;

    public string AwardingOrganisationLabel { get; init; } =  string.Empty;

    public string QualificationNumberLabel { get; init; } =  string.Empty;

    public string NotesAdditionalRequirementsLabel { get; init; } = string.Empty;

    public string ShowingAllQualificationsLabel { get; set; } =  string.Empty;

    public List<QualificationWebViewModel> Qualifications { get; init; } = [];

    public string NoQualificationsFoundContent { get; init; } = string.Empty;

    public string CheckIfAnEarlyYearsQualificationIsFullAndRelevantContent { get; init; } = string.Empty;

    public string MultipleQualificationsFoundText { get; init; } = string.Empty;

    public string SingleQualificationFoundText { get; init; } = string.Empty;

    // Filters content
    public string FilterHeading { get; init; } = string.Empty;

    public string SelectedFiltersHeading { get; init; } = string.Empty;

    public string KeywordHeading { get; init; } = string.Empty;

    public string QualificationStartDateHeading { get; init; } = string.Empty;

    public string QualificationLevelHeading { get; init; } = string.Empty;

    public string ApplyFiltersButtonContent { get; init; } = string.Empty;

    public string ClearFiltersLinkLabel { get; init; } = string.Empty;

    public string NoFiltersSelectedContent { get; init; } = string.Empty;

    // Filters
    public List<IOptionItemModel> StartDateFilters { get; set; } = [];

    public List<IOptionItemModel> LevelFilters { get; set; } = [];

    public string QualificationLevelFilter { get; init; } = string.Empty;

    public string QualificationStartDateFilter { get; init; } = string.Empty;

    public string SearchTermFilter { get; init; } = string.Empty;

    public bool HasFilters
    {
        get
        {
            return !string.IsNullOrWhiteSpace(SearchTermFilter) ||
                   !string.IsNullOrWhiteSpace(QualificationStartDateFilter) ||
                   !string.IsNullOrWhiteSpace(QualificationLevelFilter);
        }
    }
}
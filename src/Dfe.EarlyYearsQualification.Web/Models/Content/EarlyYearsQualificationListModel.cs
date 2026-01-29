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

    // Filters
    public List<EnquiryOptionModel> StartDateFilters { get; init; } =
    [
        new()
        {
            Label = "Before September 2014",
            Value = "Before September 2014"
        },
        new()
        {
            Label = "On or after September 2014",
            Value = "On or after September 2014"
        },
        new()
        {
            Label = "On or after September 2024",
            Value = "On or after September 2024"
        }
    ];

    public List<EnquiryOptionModel> LevelFilters { get; init; } =
    [
        new()
        {
            Label = "Level 2",
            Value = "2"
        },
        new()
        {
            Label = "Level 3",
            Value = "3"
        },
        new()
        {
            Label = "Level 4",
            Value = "4"
        },
        new()
        {
            Label = "Level 5",
            Value = "5"
        },
        new()
        {
            Label = "Level 6",
            Value = "6"
        },
        new()
        {
            Label = "Level 7",
            Value = "7"
        }
    ];

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
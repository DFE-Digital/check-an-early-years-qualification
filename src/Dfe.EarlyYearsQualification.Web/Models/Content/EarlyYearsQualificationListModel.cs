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

    public string AwardingOrganisationLabel { get; init; } =  string.Empty;

    public string QualificationNumberLabel { get; init; } =  string.Empty;

    public string NotesAdditionalRequirementsLabel { get; init; } = string.Empty;

    public List<BasicQualificationModel> Qualifications { get; init; } = [];
}
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class QualificationDetailsPageViewModel
{
    // Contentful fields
    public NavigationLinkModel? BackButton { get; set; } = new();

    public string Heading { get; set; } = string.Empty;
    
    public string PostHeadingContent { get; set; } = string.Empty;

    public string CtaButtonText { get; set; } = string.Empty;

    public string QualificationNameHeading { get; set; } = string.Empty;

    public string QualificationNameErrorMessage { get; set; } = string.Empty;

    public string AwardingOrganisationHeading { get; set; } = string.Empty;

    public string ErrorBannerHeading { get; set; } = string.Empty;

    public string AwardingOrganisationErrorMessage { get; set; } = string.Empty;

    // text inputs
    [Required]
    [IncludeInTelemetry]
    public string QualificationName { get; set; } = string.Empty;

    // Date inputs
    public int? StartDateSelectedMonth { get; set; }

    public int? StartDateSelectedYear { get; set; }

    public DatesQuestionModel QuestionModel { get; set; } = new DatesQuestionModel();

    public BaseDateQuestionModel OptionalQualificationStartDate { get; set; } = new BaseDateQuestionModel();

    public DateQuestionModel QualificationAwardedDate { get; set; } = new DateQuestionModel();

    // awarding organisation input

    [Required]
    [IncludeInTelemetry]
    public string AwardingOrganisation { get; set; } = string.Empty;

    // validation handling
    public bool HasAwardingOrganisationError { get; set; }

    public bool HasQualificationNameError { get; set; }

    public bool HasRequiredAwardedDateMonthError { get; set; }

    public bool HasRequiredAwardedDateYearError { get; set; }

    public bool HasValidationErrors => Errors.Any();

    public List<ErrorSummaryLink> Errors { get; set; } = new();

    public ErrorSummaryModel ErrorSummaryModel => new ErrorSummaryModel
    {
        ErrorBannerHeading = ErrorBannerHeading,
        ErrorSummaryLinks = Errors
    };
}
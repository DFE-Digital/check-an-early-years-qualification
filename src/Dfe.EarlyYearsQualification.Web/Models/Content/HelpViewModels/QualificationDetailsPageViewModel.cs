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

    public string MissingStartedDateOptionErrorMessage { get; set; } = string.Empty;

    public DateQuestionModel AwardedDate { get; set; } = new DateQuestionModel();

    public OptionModel Before2014Option { get; set; } = new OptionModel();

    public RadioButtonAndDateInputModel RadioButtonWithDateInputModel { get; set; } = new RadioButtonAndDateInputModel() { Question = new DateQuestionModel() };

    [Required]
    [IncludeInTelemetry]
    public string QualificationName { get; set; } = string.Empty;

    [Required]
    [IncludeInTelemetry]
    public string AwardingOrganisation { get; set; } = string.Empty;

    [Required]
    [IncludeInTelemetry]
    public string Option { get; set; } = string.Empty;

    // validation handling
    public bool HasQualificationNameError { get; set; }

    public bool HasOptionError { get; set; }

    public bool HasAwardingOrganisationError { get; set; }

    public Dictionary<string, object> GetQualificationNameInputAttributes()
    {
        var attributes = new Dictionary<string, object>
                        {
                            { "class", "govuk-input" },
                            { "autocomplete", "off" },
                        };

        if (HasQualificationNameError)
        {
            attributes.Add("aria-describedby", "qualification-name-error");
            attributes["class"] += " govuk-input--error";
        }

        return attributes;
    }

    public Dictionary<string, object> GetOptionInputAttributes()
    {
        var attributes = new Dictionary<string, object>
                        {
                            { "id", Before2014Option.Value },
                            { "class", "govuk-radios__input" },
                            { "autocomplete", "off" },
                        };

        if (HasOptionError)
        {
            attributes.Add("aria-describedby", "option-error");
        }

        return attributes;
    }

    public Dictionary<string, object> GetAwardingOrganisationInputAttributes()
    {
        var attributes = new Dictionary<string, object>
                        {
                            { "class", "govuk-input" },
                            { "autocomplete", "off" },
                        };

        if (HasAwardingOrganisationError)
        {
            attributes.Add("aria-describedby", "qualification-organisation-error");
            attributes["class"] += " govuk-input--error";
        }

        return attributes;
    }

    public bool HasValidationErrors => Errors.Count > 0;

    public List<ErrorSummaryLink> Errors { get; set; } = new();

    public ErrorSummaryModel ErrorSummaryModel => new ErrorSummaryModel
    {
        ErrorBannerHeading = ErrorBannerHeading,
        ErrorSummaryLinks = Errors
    };
}
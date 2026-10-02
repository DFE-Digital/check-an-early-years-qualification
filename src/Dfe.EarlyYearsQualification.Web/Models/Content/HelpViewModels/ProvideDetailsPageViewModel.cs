using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class ProvideDetailsPageViewModel
{
    // Contentful fields
    public NavigationLinkModel? BackButton { get; init; } = new();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;
    
    public string CtaButtonText { get; init; } = string.Empty;

    public string AdditionalInformationWarningText { get; init; } = string.Empty;

    // text area input
    [Required]
    [AppSettingsMaxLength("Help:MessageCharacterLimit")]
    public string ProvideAdditionalInformation { get; set; } = string.Empty;

    // validation handling
    public bool HasValidationErrors => Errors.Count > 0;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string AdditionalInformationErrorMessage { get; init; } = string.Empty;

    public bool HasAdditionalInformationError { get; set; }

    public Dictionary<string, object> GetAdditionalInformationInputAttributes()
    {
        var attributes = new Dictionary<string, object>
                        {
                            { "class", "govuk-textarea" },
                            { "autocomplete", "off" },
                            { "aria-describedby", "additional-information-hint warning-text-container" },
                        };

        if (HasAdditionalInformationError)
        {
            attributes["aria-describedby"] += " additional-information-error";
            attributes["class"] += " govuk-input--error";
        }

        return attributes;
    }

    List<ErrorSummaryLink> Errors
    {
        get
        {
            var errors = new List<ErrorSummaryLink>();

            if (HasAdditionalInformationError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = AdditionalInformationErrorMessage,
                        ElementLinkId = "ProvideAdditionalInformation"
                    }
                );
            }

            return errors;
        }
    }

    public ErrorSummaryModel ErrorSummaryModel => new ErrorSummaryModel
    {
        ErrorBannerHeading = ErrorBannerHeading,
        ErrorSummaryLinks = Errors
    };
    
    public string StaticCharactersRemainingMessage { get; init; } = string.Empty;

    public string DynamicCharactersRemainingMessage { get; init; } = string.Empty;

    public string SingularCharacterRemainingMessage { get; init; } = string.Empty;

    public string DynamicTooManyCharactersEnteredMessage { get; init; } = string.Empty;

    public string SingularTooManyCharactersEnteredMessage { get; init; } = string.Empty;

    public string TooManyCharactersEnteredErrorMessage { get; init; } = string.Empty;

    public int MaxCharacterLimit { get; init; }
}
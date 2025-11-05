using System.ComponentModel.DataAnnotations;

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
    public string ProvideAdditionalInformation { get; set; } = string.Empty;

    // validation handling
    public bool HasValidationErrors => Errors.Count > 0;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string AdditionalInformationErrorMessage { get; init; } = string.Empty;

    public bool HasAdditionalInformationError { get; set; }

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
}
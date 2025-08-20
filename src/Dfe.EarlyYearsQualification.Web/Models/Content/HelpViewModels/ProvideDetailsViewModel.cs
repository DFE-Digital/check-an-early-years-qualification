using Dfe.EarlyYearsQualification.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class ProvideDetailsViewModel
{
    public NavigationLinkModel? BackButton { get; init; } = new()
    {
        DisplayText = "Back to get help with the Check an early years qualification service",
        Href = "./qualification-details"
    };

    public string Heading { get; init; } = "How can we help you?";
    
    public string PostHeadingContent { get; init; } = "Give as much detail as you can. This helps us give you the right support.";
    
    public string CtaButtonText { get; init; } = "Continue";

    public string AdditionalInformationWarningText { get; init; } = "Do not include any personal information";

    // text area input
    [Required]
    [IncludeInTelemetry]
    public string ProvideAdditionalInformation { get; set; } = string.Empty;

    public bool HasAdditionalInformationError { get; set; }

    public string AdditionalInformationErrorMessage { get; init; } = "Provide information about how we can help you";

    // validation handling
    public string ErrorBannerHeading { get; init; } = "There is a problem";

    public bool HasValidationErrors => Errors.Any();

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
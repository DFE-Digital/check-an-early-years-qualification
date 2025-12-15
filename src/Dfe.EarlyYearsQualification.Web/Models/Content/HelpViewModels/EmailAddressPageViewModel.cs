using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class EmailAddressPageViewModel
{
    // Contentful fields
    public NavigationLinkModel BackButton { get; set; } = new NavigationLinkModel();

    public string CtaButtonText { get; set; } = string.Empty;

    public string Heading { get; set; } = string.Empty;

    public string PostHeadingContent { get; set; } = string.Empty;

    public string EmailAddressErrorMessage { get; set; } = string.Empty;

    public string ErrorBannerHeading { get; set; } = string.Empty;

    // values to bind
    [EmailAddress]
    [Required]
    [Sensitive]
    [IncludeInTelemetry]
    public string EmailAddress { get; set; } = string.Empty;

    // validation handling
    public bool HasEmailAddressError { get; set; }

    public bool HasValidationErrors => Errors.Count > 0;

    List<ErrorSummaryLink> Errors
    {
        get
        {
            var errors = new List<ErrorSummaryLink>();

            if (HasEmailAddressError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = EmailAddressErrorMessage,
                        ElementLinkId = "EmailAddress"
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
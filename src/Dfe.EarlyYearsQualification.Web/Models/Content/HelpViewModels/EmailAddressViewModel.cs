using Dfe.EarlyYearsQualification.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class EmailAddressViewModel
{
    public NavigationLinkModel? BackButton { get; init; } = new()
    {
        DisplayText = "Back to get help with the Check an early years qualification service",
        Href = "./get-help"
    };

    public string CtaButtonText { get; init; } = "Send a message";

    // email address input
    public string EmailAddressHeading { get; init; } = "What is your email address?";

    public string EmailAddressHintText { get; init; } = "We will only use this email address to reply to your message";

    public bool HasEmailAddressError { get; set; }

    public string EmailAddressErrorMessage { get; set; } = "Enter an email address";

    [EmailAddress]
    [Required]
    [Sensitive]
    [IncludeInTelemetry]
    public string EmailAddress { get; set; } = string.Empty;

    // validation handling
    public string ErrorBannerHeading { get; init; } = "There is a problem";

    public bool HasValidationErrors => Errors.Any();

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
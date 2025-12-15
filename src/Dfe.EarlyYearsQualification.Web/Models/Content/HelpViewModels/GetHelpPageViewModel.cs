using System.ComponentModel.DataAnnotations;
using Dfe.EarlyYearsQualification.Web.Attributes;

namespace Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

public class GetHelpPageViewModel
{
    // Contentful fields
    public NavigationLinkModel BackButton { get; init; } = new NavigationLinkModel();

    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;
    
    public string ReasonForEnquiryHeading { get; init; } = string.Empty;

    public string CtaButtonText { get; init; } = string.Empty;

    public List<EnquiryOptionModel> EnquiryReasons { get; init; } = new List<EnquiryOptionModel>();

    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = string.Empty;

    public string ErrorBannerHeading { get; init; } = string.Empty;

    // values to bind
    [Required]
    [IncludeInTelemetry]
    public string SelectedOption { get; set; } = string.Empty;

    // validation handling
    public bool HasNoEnquiryOptionSelectedError { get; set; }

    public bool HasValidationErrors => Errors.Count > 0;

    List<ErrorSummaryLink> Errors
    {
        get
        {
            var errors = new List<ErrorSummaryLink>();

            if (HasNoEnquiryOptionSelectedError)
            {
                errors.Add(
                    new ErrorSummaryLink
                    {
                        ErrorBannerLinkText = NoEnquiryOptionSelectedErrorMessage,
                        ElementLinkId = EnquiryReasons[0].Value
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
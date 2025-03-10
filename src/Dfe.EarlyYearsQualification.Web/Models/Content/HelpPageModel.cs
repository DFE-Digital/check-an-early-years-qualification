using System.ComponentModel.DataAnnotations;

namespace Dfe.EarlyYearsQualification.Web.Models.Content;

public class HelpPageModel
{
    public string Heading { get; init; } = string.Empty;
    
    public string PostHeadingContent { get; init; } = string.Empty;
    
    public string EmailAddressHeading { get; init; } = string.Empty;

    public string EmailAddressHintText { get; init; } = string.Empty;

    public string ReasonForEnquiryHeading { get; init; } = string.Empty;

    public string ReasonForEnquiryHintText { get; init; } = string.Empty;

    public List<EnquiryOptionModel> EnquiryReasons { get; init; } = [];

    public string AdditionalInformationHeading { get; init; } = string.Empty;
    
    public string AdditionalInformationHintText { get; init; } = string.Empty;
    
    public string AdditionalInformationWarningText { get; init; } = string.Empty;
    
    public string CtaButtonText { get; init; } = string.Empty;

    public NavigationLinkModel? BackButton { get; init; } = new();

    public bool HasErrors
    {
        get
        {
            return HasFurtherInformationError || HasInvalidEmailAddressError || HasNoEnquiryOptionSelectedError;
        }
    }

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public bool HasInvalidEmailAddressError { get; set; }

    public bool HasNoEnquiryOptionSelectedError { get; set; }

    public bool HasFurtherInformationError { get; set; }

    public string InvalidEmailAddressErrorMessage { get; init; } = string.Empty;
    
    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = string.Empty;
    
    public string FurtherInformationErrorMessage { get; init; } = string.Empty;

    [Required]
    public string SelectedOption { get; set; } = string.Empty;

    [EmailAddress]
    public string? EmailAddress { get; set; } = string.Empty;

    [Required]
    public string AdditionalInformationMessage { get; set; } = string.Empty;
}
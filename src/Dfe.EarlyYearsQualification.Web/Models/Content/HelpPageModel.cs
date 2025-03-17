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
            return HasFurtherInformationError || HasEmailAddressError || HasNoEnquiryOptionSelectedError;
        }
    }

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public bool HasEmailAddressError { get; set; }

    public bool HasNoEnquiryOptionSelectedError { get; set; }

    public bool HasFurtherInformationError { get; set; }

    public string EmailAddressErrorMessage { get; set; } = string.Empty;
    
    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = string.Empty;
    
    public string AdditionalInformationErrorMessage { get; init; } = string.Empty;

    [Required]
    public string SelectedOption { get; set; } = string.Empty;

    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string AdditionalInformationMessage { get; set; } = string.Empty;
}
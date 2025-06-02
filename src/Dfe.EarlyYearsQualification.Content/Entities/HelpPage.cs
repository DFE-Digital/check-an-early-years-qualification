using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Entities;

public class HelpPage
{
    public string Heading { get; init; } = string.Empty;
    
    public Document PostHeadingContent { get; init; } = new();
    
    public string EmailAddressHeading { get; init; } = string.Empty;

    public string EmailAddressHintText { get; init; } = string.Empty;

    public string ReasonForEnquiryHeading { get; init; } = string.Empty;

    public string ReasonForEnquiryHintText { get; init; } = string.Empty;

    public List<EnquiryOption> EnquiryReasons { get; init; } = [];

    public string AdditionalInformationHeading { get; init; } = string.Empty;
    
    public string AdditionalInformationHintText { get; init; } = string.Empty;
    
    public string AdditionalInformationWarningText { get; init; } = string.Empty;
    
    public string CtaButtonText { get; init; } = string.Empty;

    public NavigationLink BackButton { get; init; } = new();

    public string ErrorBannerHeading { get; init; } = string.Empty;

    public string NoEmailAddressEnteredErrorMessage { get; init; } = string.Empty;

    public string InvalidEmailAddressErrorMessage { get; init; } = string.Empty;
    
    public string NoEnquiryOptionSelectedErrorMessage { get; init; } = string.Empty;
    
    public string FurtherInformationErrorMessage { get; init; } = string.Empty;
}
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class HelpPageMapper
{
    public static HelpPageModel Map(HelpPage helpPage, string postHeadingContent)
    {
        return new HelpPageModel
               {
                   Heading = helpPage.Heading,
                   PostHeadingContent = postHeadingContent,
                   AdditionalInformationHeading = helpPage.AdditionalInformationHeading,
                   AdditionalInformationHintText = helpPage.AdditionalInformationHintText,
                   AdditionalInformationWarningText = helpPage.AdditionalInformationWarningText,
                   BackButton = NavigationLinkMapper.Map(helpPage.BackButton),
                   CtaButtonText = helpPage.CtaButtonText,
                   EmailAddressHeading = helpPage.EmailAddressHeading,
                   AdditionalInformationErrorMessage = helpPage.FurtherInformationErrorMessage,
                   EmailAddressHintText = helpPage.EmailAddressHintText,
                   ReasonForEnquiryHeading = helpPage.ReasonForEnquiryHeading,
                   ErrorBannerHeading = helpPage.ErrorBannerHeading,
                   InvalidEmailAddressErrorMessage = helpPage.InvalidEmailAddressErrorMessage,
                   ReasonForEnquiryHintText = helpPage.ReasonForEnquiryHintText,
                   NoEnquiryOptionSelectedErrorMessage = helpPage.NoEnquiryOptionSelectedErrorMessage,
                   EnquiryReasons = MapEnquiryReasons(helpPage.EnquiryReasons)
               };
    }

    private static List<EnquiryOptionModel> MapEnquiryReasons(List<EnquiryOption> helpPageEnquiryReasons)
    {
        var results = new List<EnquiryOptionModel>();
        if (helpPageEnquiryReasons.Count == 0)
        {
            return results;
        }

        foreach (var enquiryReason in helpPageEnquiryReasons)
        {
            results.Add(new EnquiryOptionModel { Label = enquiryReason.Label, Value = enquiryReason.Value });
        }

        return results;
    }
}
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class HelpPageMapper(IGovUkContentParser contentParser) : IHelpPageMapper
{
    public async Task<HelpPageModel> Map(HelpPage helpPage, string emailAddressErrorMessage)
    {
        var postHeadingContent = await contentParser.ToHtml(helpPage.PostHeadingContent);
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
                   ReasonForEnquiryHintText = helpPage.ReasonForEnquiryHintText,
                   EmailAddressErrorMessage = emailAddressErrorMessage,
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
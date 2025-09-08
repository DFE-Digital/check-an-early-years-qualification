using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpGetHelpPageMapper(IGovUkContentParser contentParser) : IHelpGetHelpPageMapper
{
    public async Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage helpPageContent)
    {
        var viewModel = new GetHelpPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = helpPageContent.BackButton.DisplayText,
                Href = helpPageContent.BackButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = await contentParser.ToHtml(helpPageContent.PostHeadingContent),
            CtaButtonText = helpPageContent.CtaButtonText,
            EnquiryReasons = MapEnquiryReasons(helpPageContent.EnquiryReasons),
            NoEnquiryOptionSelectedErrorMessage = helpPageContent.NoEnquiryOptionSelectedErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
            ReasonForEnquiryHeading = helpPageContent.ReasonForEnquiryHeading,
        };

        return viewModel;
    }

    private static List<EnquiryOptionModel> MapEnquiryReasons(List<EnquiryOption> helpPageEnquiryReasons)
    {
        var results = new List<EnquiryOptionModel>();

        foreach (var enquiryReason in helpPageEnquiryReasons)
        {
            results.Add(new EnquiryOptionModel { Label = enquiryReason.Label, Value = enquiryReason.Value });
        }

        return results;
    }
}
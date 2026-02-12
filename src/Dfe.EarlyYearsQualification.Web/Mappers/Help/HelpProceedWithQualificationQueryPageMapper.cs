using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpProceedWithQualificationQueryPageMapper(IGovUkContentParser contentParser) : IHelpProceedWithQualificationQueryPageMapper
{
    public async Task<ProceedWithQualificationQueryViewModel> MapProceedWithQualificationQueryPageContentToViewModelAsync(HelpProceedWithQualificationQueryPage content)
    {
        var viewModel = new ProceedWithQualificationQueryViewModel
        {
            BackButton = new NavigationLinkModel
            {
                DisplayText = content.BackButton.DisplayText,
                Href = content.BackButton.Href
            },
            Heading = content.Heading,
            PostHeadingContent = await contentParser.ToHtml(content.PostHeadingContent),
            CtaButtonText = content.CtaButtonText,
            EnquiryReasons = EnquiryReasonsMapper.Map(content.EnquiryReasons),
            NoEnquiryOptionSelectedErrorMessage = content.NoEnquiryOptionSelectedErrorMessage,
            ErrorBannerHeading = content.ErrorBannerHeading,
            ReasonForEnquiryHeading = content.ReasonForEnquiryHeading,
        };

        return viewModel;
    }
}
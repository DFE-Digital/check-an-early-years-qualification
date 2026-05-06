using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpConfirmationPageMapper(IGovUkContentParser contentParser, IFeedbackFormPageMapper feedbackFormPageMapper) : IHelpConfirmationPageMapper
{
    public async Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage helpConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        var postFeedbackFormContentHtml = await contentParser.ToHtml(helpConfirmationPage.PostFeedbackFormContent);

        return new ConfirmationPageViewModel
        {
            SuccessMessage = helpConfirmationPage.SuccessMessage,
            BodyHeading = helpConfirmationPage.BodyHeading,
            Body = bodyHtml,
            ReturnToTheHomepageLink = NavigationLinkMapper.Map(helpConfirmationPage.ReturnToHomepageLink),
            SuccessMessageFollowingText = helpConfirmationPage.SuccessMessageFollowingText,
            FeedbackFormPageModel = await feedbackFormPageMapper.Map(helpConfirmationPage.FeedbackFormPage!),
            PostFeedbackFormContent = postFeedbackFormContentHtml,
        };
    }
}
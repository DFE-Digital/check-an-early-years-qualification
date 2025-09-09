using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers.Help;

public class HelpConfirmationPageMapper(IGovUkContentParser contentParser) : IHelpConfirmationPageMapper
{
    public async Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage helpConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        var feedbackBodyHtml = await contentParser.ToHtml(helpConfirmationPage.FeedbackComponent!.Body);

        return new ConfirmationPageViewModel
        {
            SuccessMessage = helpConfirmationPage.SuccessMessage,
            BodyHeading = helpConfirmationPage.BodyHeading,
            Body = bodyHtml,
            FeedbackComponent = FeedbackComponentModelMapper.Map(helpConfirmationPage.FeedbackComponent!.Header, feedbackBodyHtml),
            ReturnToTheHomepageLink = NavigationLinkMapper.Map(helpConfirmationPage.ReturnToHomepageLink),
            SuccessMessageFollowingText = helpConfirmationPage.SuccessMessageFollowingText
        };
    }
}
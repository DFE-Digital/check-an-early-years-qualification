using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class HelpConfirmationPageModelMapper
{
    public static ConfirmationPageViewModel Map(HelpConfirmationPage helpConfirmationPage, string bodyHtml,
                                                string feedbackBodyHtml)
    {
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
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class HelpConfirmationPageModelMapper
{
    public static HelpConfirmationPageModel Map(HelpConfirmationPage helpConfirmationPage, string bodyHtml,
                                                string feedbackBodyHtml)
    {
        return new HelpConfirmationPageModel
               {
                   SuccessMessage = helpConfirmationPage.SuccessMessage,
                   BodyHeading = helpConfirmationPage.BodyHeading,
                   Body = bodyHtml,
                   FeedbackComponent = FeedbackComponentMapper.Map(helpConfirmationPage.FeedbackComponent!.Header, feedbackBodyHtml),
                   ReturnToTheHomepageLink = NavigationLinkMapper.Map(helpConfirmationPage.ReturnToHomepageLink)
               };
    }
}
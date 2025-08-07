using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FeedbackFormConfirmationPageMapper
{
    public static FeedbackFormConfirmationPageModel Map(FeedbackFormConfirmationPage feedbackFormConfirmationPage, string bodyHtml, string optionEmailBodyHtml)
    {
        return new FeedbackFormConfirmationPageModel
               {
                   SuccessMessage = feedbackFormConfirmationPage.SuccessMessage,
                   Body = bodyHtml,
                   OptionalEmailHeading = feedbackFormConfirmationPage.OptionalEmailHeading,
                   OptionalEmailBody = optionEmailBodyHtml,
                   ReturnToHomepageLink = NavigationLinkMapper.Map(feedbackFormConfirmationPage.ReturnToHomepageLink)
               };
    }
}
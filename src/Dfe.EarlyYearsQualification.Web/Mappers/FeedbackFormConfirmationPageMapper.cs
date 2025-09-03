using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class FeedbackFormConfirmationPageMapper(IGovUkContentParser contentParser) : IFeedbackFormConfirmationPageMapper
{
    public async Task<FeedbackFormConfirmationPageModel> Map(FeedbackFormConfirmationPage feedbackFormConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(feedbackFormConfirmationPage.Body);
        var optionEmailBodyHtml = await contentParser.ToHtml(feedbackFormConfirmationPage.OptionalEmailBody);
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
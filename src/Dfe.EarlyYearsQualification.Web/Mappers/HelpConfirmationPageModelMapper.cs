using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class HelpConfirmationPageModelMapper(IGovUkContentParser contentParser) : IHelpConfirmationPageModelMapper
{
    public async Task<HelpConfirmationPageModel> Map(HelpConfirmationPage helpConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        var feedbackBodyHtml = await contentParser.ToHtml(helpConfirmationPage.FeedbackComponent!.Body);
        
        return new HelpConfirmationPageModel
               {
                   SuccessMessage = helpConfirmationPage.SuccessMessage,
                   BodyHeading = helpConfirmationPage.BodyHeading,
                   Body = bodyHtml,
                   FeedbackComponent = FeedbackComponentModelMapper.Map(helpConfirmationPage.FeedbackComponent!.Header, feedbackBodyHtml),
                   ReturnToTheHomepageLink = NavigationLinkMapper.Map(helpConfirmationPage.ReturnToHomepageLink)
               };
    }
}
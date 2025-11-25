using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class AdvicePageMapper(IGovUkContentParser contentParser) : IAdvicePageMapper
{
    public async Task<AdvicePageModel> Map(AdvicePage advicePage)
    {
        var bodyHtml = await contentParser.ToHtml(advicePage.Body);
        var improveServiceBodyHtml = advicePage.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(advicePage.UpDownFeedback.FeedbackComponent!.Body)
                                         : null;
        var rightHandSideContentHtml = advicePage.RightHandSideContent is not null
                                           ? await contentParser.ToHtml(advicePage.RightHandSideContent.Body)
                                           : null;
        
        FeedbackComponentModel? rightHandSideContent = null;
        if (advicePage.RightHandSideContent != null && !string.IsNullOrEmpty(rightHandSideContentHtml))
        {
            rightHandSideContent =
                FeedbackComponentModelMapper.Map(advicePage.RightHandSideContent.Header, rightHandSideContentHtml);
        }
        return new AdvicePageModel
               {
                   Heading = advicePage.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(advicePage.BackButton),
                   UpDownFeedback = UpDownFeedbackMapper.Map(advicePage.UpDownFeedback, improveServiceBodyHtml),
                   RightHandSideContent = rightHandSideContent
               };
    }

    public async Task<QualificationNotOnListPageModel> Map(CannotFindQualificationPage cannotFindQualificationPage)
    {
        var bodyHtml = await contentParser.ToHtml(cannotFindQualificationPage.Body);
        var improveServiceBodyHtml = cannotFindQualificationPage.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(cannotFindQualificationPage.UpDownFeedback.FeedbackComponent!.Body)
                                         : null;
        var rightHandSideContentHtml = cannotFindQualificationPage.RightHandSideContent is not null
                                           ? await contentParser.ToHtml(cannotFindQualificationPage.RightHandSideContent.Body)
                                           : null;
        
        FeedbackComponentModel? rightHandSideContent = null;
        if (cannotFindQualificationPage.RightHandSideContent != null && !string.IsNullOrEmpty(rightHandSideContentHtml))
        {
            rightHandSideContent =
                FeedbackComponentModelMapper.Map(cannotFindQualificationPage.RightHandSideContent.Header, rightHandSideContentHtml);
        }
        return new QualificationNotOnListPageModel
               {
                   Heading = cannotFindQualificationPage.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(cannotFindQualificationPage.BackButton),
                   UpDownFeedback = UpDownFeedbackMapper.Map(cannotFindQualificationPage.UpDownFeedback, improveServiceBodyHtml),
                   RightHandSideContent = rightHandSideContent
               };
    }
}
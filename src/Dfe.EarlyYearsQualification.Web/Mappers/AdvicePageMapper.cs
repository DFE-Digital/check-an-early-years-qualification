using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class AdvicePageMapper
{
    public static AdvicePageModel Map(AdvicePage advicePage, string bodyHtml, string? feedbackBodyHtml, string? improveServiceBodyHtml, string? rightHandSideContentHtml)
    {
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
                   FeedbackBanner = FeedbackBannerMapper.Map(advicePage.FeedbackBanner, feedbackBodyHtml),
                   UpDownFeedback = UpDownFeedbackMapper.Map(advicePage.UpDownFeedback, improveServiceBodyHtml),
                   RightHandSideContent = rightHandSideContent
               };
    }

    public static QualificationNotOnListPageModel Map(CannotFindQualificationPage cannotFindQualificationPage, string bodyHtml,
                                                      string? feedbackBodyHtml, string? improveServiceBodyHtml, string? rightHandSideContentHtml)
    {
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
                   FeedbackBanner =
                       FeedbackBannerMapper.Map(cannotFindQualificationPage.FeedbackBanner, feedbackBodyHtml),
                   UpDownFeedback = UpDownFeedbackMapper.Map(cannotFindQualificationPage.UpDownFeedback, improveServiceBodyHtml),
                   RightHandSideContent = rightHandSideContent
               };
    }
}
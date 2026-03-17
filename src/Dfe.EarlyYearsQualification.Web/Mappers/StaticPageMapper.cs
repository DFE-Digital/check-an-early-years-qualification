using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class StaticPageMapper(IGovUkContentParser contentParser) : IStaticPageMapper
{
    public async Task<StaticPageModel> Map(StaticPage page)
    {
        var bodyHtml = await contentParser.ToHtml(page.Body);
        var improveServiceBodyHtml = page.UpDownFeedback is not null
                                         ? await contentParser.ToHtml(page.UpDownFeedback.FeedbackComponent!.Body)
                                         : null;
        var rightHandSideContentHtml = page.RightHandSideContent is not null
                                           ? await contentParser.ToHtml(page.RightHandSideContent.Body)
                                           : null;
        
        FeedbackComponentModel? rightHandSideContent = null;
        if (page.RightHandSideContent != null && !string.IsNullOrEmpty(rightHandSideContentHtml))
        {
            rightHandSideContent =
                FeedbackComponentModelMapper.Map(page.RightHandSideContent.Header, rightHandSideContentHtml);
        }
        return new StaticPageModel
               {
                   Heading = page.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(page.BackButton),
                   UpDownFeedback = UpDownFeedbackMapper.Map(page.UpDownFeedback, improveServiceBodyHtml),
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
                   RightHandSideContent = rightHandSideContent,
                   UserType = cannotFindQualificationPage.IsPractitionerSpecificPage? UserTypes.Practitioner: UserTypes.Manager
               };
    }
}
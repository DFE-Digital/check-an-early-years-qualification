using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class AccessibilityStatementMapper
{
    public static AccessibilityStatementPageModel Map(AccessibilityStatementPage content, string bodyHtml)
    {
        return new AccessibilityStatementPageModel
               {
                   Heading = content.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(content.BackButton)
               };
    }
}
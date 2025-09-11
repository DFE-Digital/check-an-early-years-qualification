using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class AccessibilityStatementMapper(IGovUkContentParser contentParser) : IAccessibilityStatementMapper
{
    public async Task<AccessibilityStatementPageModel> Map(AccessibilityStatementPage content)
    {
        var bodyHtml = await contentParser.ToHtml(content.Body);
        return new AccessibilityStatementPageModel
               {
                   Heading = content.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(content.BackButton)
               };
    }
}
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

        return new StaticPageModel
               {
                   Heading = page.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(page.BackButton),
               };
    }

    public async Task<QualificationNotOnListPageModel> Map(CannotFindQualificationPage cannotFindQualificationPage)
    {
        var bodyHtml = await contentParser.ToHtml(cannotFindQualificationPage.Body);
        
        return new QualificationNotOnListPageModel
               {
                   Heading = cannotFindQualificationPage.Heading,
                   BodyContent = bodyHtml,
                   BackButton = NavigationLinkMapper.Map(cannotFindQualificationPage.BackButton),
                   UserType = cannotFindQualificationPage.IsPractitionerSpecificPage? UserTypes.Practitioner: UserTypes.Manager
               };
    }
}
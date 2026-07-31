using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class FooterMapper(IGovUkContentParser contentParser) : IFooterMapper
{
    public async Task<FooterModel> Map(Footer footer, string? route)
    {
        var leftHandSideContentBody = footer.LeftHandSideFooterSection is not null
                                      ? await contentParser.ToHtml(footer.LeftHandSideFooterSection.Body)
                                      : null;
        var rightHandSideContentBody = footer.RightHandSideFooterSection is not null
                                       ? await contentParser.ToHtml(footer.RightHandSideFooterSection.Body)
                                       : null;

        var navigationLinks = SetNavigationLinks(footer, route);

        var result = new FooterModel
               {
                   NavigationLinks = navigationLinks,
               };

        if (footer.LeftHandSideFooterSection is not null && !string.IsNullOrEmpty(leftHandSideContentBody))
        {
            result.LeftHandSideFooterSection = new FooterSectionModel
                                                   {
                                                       Heading = footer.LeftHandSideFooterSection.Heading,
                                                       Body = leftHandSideContentBody
                                                   };
        }
        
        if (footer.RightHandSideFooterSection is not null && !string.IsNullOrEmpty(rightHandSideContentBody))
        {
            result.RightHandSideFooterSection = new FooterSectionModel
                                                    {
                                                        Heading = footer.RightHandSideFooterSection.Heading,
                                                        Body = rightHandSideContentBody
                                                    };
        }

        return result;
    }

    private static List<NavigationLinkModel?> SetNavigationLinks(Footer footer, string? route)
    {
        var routes = new HashSet<string?>
        {
            Urls.EarlyYearsQualificationList,
            Urls.AccessibilityStatementEYQL,
            Urls.ScottishQualificationLevels,
            Urls.FindAnApprovedEarlyYearsCourse,
        };

        var targetHref = routes.Contains(route) ? Urls.AccessibilityStatement : Urls.AccessibilityStatementEYQL;

        return footer.NavigationLinks
            .Select(NavigationLinkMapper.Map)
            .Where(item => item?.Href != targetHref)
            .ToList();
    }
}
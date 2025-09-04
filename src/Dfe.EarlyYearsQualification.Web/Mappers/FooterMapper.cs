using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class FooterMapper(IGovUkContentParser contentParser) : IFooterMapper
{
    public async Task<FooterModel> Map(Footer footer)
    {
        var leftHandSideContentBody = footer.LeftHandSideFooterSection is not null
                                      ? await contentParser.ToHtml(footer.LeftHandSideFooterSection.Body)
                                      : null;
        var rightHandSideContentBody = footer.RightHandSideFooterSection is not null
                                       ? await contentParser.ToHtml(footer.RightHandSideFooterSection.Body)
                                       : null;
        
        var result = new FooterModel
               {
                   NavigationLinks = footer.NavigationLinks
                                           .Select(navigationLink => NavigationLinkMapper.Map(navigationLink)).ToList(),
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
}
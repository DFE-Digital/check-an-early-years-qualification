using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class FooterMapper
{
    public static FooterModel Map(Footer footer, string? leftHandSideSectionBody, string? rightHandSideSectionBody)
    {
        var result = new FooterModel
               {
                   NavigationLinks = footer.NavigationLinks
                                           .Select(navigationLink => NavigationLinkMapper.Map(navigationLink)).ToList(),
               };
        
        if (footer.LeftHandSideFooterSection is not null && !string.IsNullOrEmpty(leftHandSideSectionBody))
        {
            result.LeftHandSideFooterSection = new FooterSectionModel
                                                   {
                                                       Heading = footer.LeftHandSideFooterSection.Heading,
                                                       Body = leftHandSideSectionBody
                                                   };
        }
        
        if (footer.RightHandSideFooterSection is not null && !string.IsNullOrEmpty(rightHandSideSectionBody))
        {
            result.RightHandSideFooterSection = new FooterSectionModel
                                                    {
                                                        Heading = footer.RightHandSideFooterSection.Heading,
                                                        Body = rightHandSideSectionBody
                                                    };
        }

        return result;
    }
}
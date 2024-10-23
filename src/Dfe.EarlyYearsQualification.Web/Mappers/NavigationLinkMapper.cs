using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class NavigationLinkMapper
{
    public static NavigationLinkModel? Map(NavigationLink? navigationLink)
    {
        if (navigationLink is null) return null;
        
        return new NavigationLinkModel
               {
                   Href = navigationLink.Href,
                   DisplayText = navigationLink.DisplayText,
                   OpenInNewTab = navigationLink.OpenInNewTab
               };
    }
}
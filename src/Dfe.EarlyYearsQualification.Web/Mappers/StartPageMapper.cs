using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public static class StartPageMapper
{
    public static StartPageModel Map(StartPage startPageContent,
                              string preCtaButtonContentHtml,
                              string postCtaButtonContentHtml,
                              string rightHandSideContentHtml)
    {
        return new StartPageModel
               {
                   Header = startPageContent.Header,
                   PreCtaButtonContent = preCtaButtonContentHtml,
                   CtaButtonText = startPageContent.CtaButtonText,
                   PostCtaButtonContent = postCtaButtonContentHtml,
                   RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
                   RightHandSideContent = rightHandSideContentHtml
               };
    }
}
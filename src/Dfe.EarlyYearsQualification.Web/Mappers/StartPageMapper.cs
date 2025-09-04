using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.Web.Mappers;

public class StartPageMapper(IGovUkContentParser contentParser) : IStartPageMapper
{
    public async Task<StartPageModel> Map(StartPage startPageContent)
    {
        var preCtaButtonContent = await contentParser.ToHtml(startPageContent.PreCtaButtonContent);
        var postCtaButtonContent = await contentParser.ToHtml(startPageContent.PostCtaButtonContent);
        var rightHandSideContent = await contentParser.ToHtml(startPageContent.RightHandSideContent);
        return new StartPageModel
               {
                   Header = startPageContent.Header,
                   PreCtaButtonContent = preCtaButtonContent,
                   CtaButtonText = startPageContent.CtaButtonText,
                   PostCtaButtonContent = postCtaButtonContent,
                   RightHandSideContentHeader = startPageContent.RightHandSideContentHeader,
                   RightHandSideContent = rightHandSideContent
               };
    }
}
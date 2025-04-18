using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class SuccessBannerRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        return await NoGovUkParagraphHelper.Render(content);
    }

    public bool SupportsContent(IContent content)
    {
        var paragraph = content as Paragraph;

        return paragraph?.NodeType == "SuccessBannerContent";
    }
}
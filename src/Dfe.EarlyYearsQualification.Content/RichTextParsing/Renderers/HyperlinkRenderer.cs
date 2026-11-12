using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class HyperlinkRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        var hyperlink = (Hyperlink)content;
        var linkText = await NestedContentHelper.Render(hyperlink.Content);
        return $"<a href='{hyperlink.Data.Uri}' class='govuk-link'>{linkText}</a>";
    }

    public bool SupportsContent(IContent content)
    {
        return content is Hyperlink;
    }
}

using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class HyperlinkRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var hyperlink = content as Hyperlink;
        var hyperlinkText = hyperlink!.Content[0] as Text;
        return Task.FromResult($"<a href='{hyperlink.Data.Uri}' class='govuk-link'>{hyperlinkText!.Value}</a>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is Hyperlink;
    }
}
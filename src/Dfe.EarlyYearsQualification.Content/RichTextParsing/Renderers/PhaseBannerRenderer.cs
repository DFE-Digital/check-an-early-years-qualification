using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class PhaseBannerRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        var paragraph = content as Paragraph;

        var sb = new StringBuilder();

        var externalLinkRenderer = new ExternalNavigationLinkRenderer();
        
        foreach (var item in paragraph!.Content)
        {
            switch (item)
            {
                case Hyperlink hl:
                    var hyperlinkRenderer = new HyperlinkRenderer();
                    var hyperlinkText = await hyperlinkRenderer.RenderAsync(hl);
                    sb.Append(hyperlinkText);
                    continue;

                case Text t:
                    sb.Append(t.Value);
                    continue;
            }

            if (externalLinkRenderer.SupportsContent(item))
            {
                sb.Append(await externalLinkRenderer.RenderAsync(item));
            }
            
        }

        return sb.ToString();
    }

    public bool SupportsContent(IContent content)
    {
        var paragraph = content as Paragraph;
        
        return paragraph?.NodeType == nameof(PhaseBanner);
    }
}
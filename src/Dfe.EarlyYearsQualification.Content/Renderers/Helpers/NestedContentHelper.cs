using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Helpers;

public static class NestedContentHelper
{
    public static async Task<string> Render(List<IContent> content)
    {
        var sb = new StringBuilder();

        foreach (var item in content)
        {
            switch (item)
            {
                case Paragraph p:
                    var paragraphRenderer = new ParagraphRenderer();
                    var paragraphText = await paragraphRenderer.RenderAsync(p);
                    sb.Append(paragraphText);
                    continue;

                case Hyperlink hl:
                    var hyperlinkRenderer = new HyperlinkRenderer();
                    var hyperlinkText = await hyperlinkRenderer.RenderAsync(hl);
                    sb.Append(hyperlinkText);
                    continue;

                case Text t:
                    sb.Append(t.Value);
                    continue;
                
                case EntryStructure cn:
                    if (new ExternalNavigationLinkRenderer().SupportsContent(cn))
                    {
                        var renderer = new ExternalNavigationLinkRenderer();
                        var text = await renderer.RenderAsync(cn);
                        sb.Append(text);
                    }
                    continue;
            }
        }

        return sb.ToString();
    }
}
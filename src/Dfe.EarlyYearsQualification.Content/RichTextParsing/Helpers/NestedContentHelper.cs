using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

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
                    if (t.Marks?.Exists(mark => mark.Type == "bold") == true)
                    {
                        sb.Append("<b>");
                        sb.Append(t.Value);
                        sb.Append("</b>");
                        continue;
                    }
                    
                    sb.Append(t.Value);
                    continue;
                
                case EntryStructure cn:
                    
                    if (new ExternalNavigationLinkRenderer().SupportsContent(cn))
                    {
                        var renderer = new ExternalNavigationLinkRenderer();
                        var text = await renderer.RenderAsync(cn);
                        sb.Append(text);
                    }

                    if (new MailtoLinkRenderer().SupportsContent(cn))
                    {
                        var renderer = new MailtoLinkRenderer();
                        var text = await renderer.RenderAsync(cn);
                        sb.Append(text);
                    }
                    
                    continue;
            }
        }

        return sb.ToString();
    }
}
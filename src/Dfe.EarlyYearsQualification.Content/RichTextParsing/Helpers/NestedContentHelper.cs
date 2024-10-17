using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

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
                case Heading1 h1:
                    var heading1Renderer = new Heading1Renderer();
                    var heading1Text = await heading1Renderer.RenderAsync(h1);
                    sb.Append(heading1Text);
                    continue;
                
                case Heading2 h2:
                    var heading2Renderer = new Heading2Renderer();
                    var heading2Text = await heading2Renderer.RenderAsync(h2);
                    sb.Append(heading2Text);
                    continue;
                
                case Heading3 h3:
                    var heading3Renderer = new Heading3Renderer();
                    var heading3Text = await heading3Renderer.RenderAsync(h3);
                    sb.Append(heading3Text);
                    continue;
                
                case Heading4 h4:
                    var heading4Renderer = new Heading4Renderer();
                    var heading4Text = await heading4Renderer.RenderAsync(h4);
                    sb.Append(heading4Text);
                    continue;
                
                case Heading5 h5:
                    var heading5Renderer = new Heading5Renderer();
                    var heading5Text = await heading5Renderer.RenderAsync(h5);
                    sb.Append(heading5Text);
                    continue;
                
                case Heading6 h6:
                    var heading6Renderer = new Heading6Renderer();
                    var heading6Text = await heading6Renderer.RenderAsync(h6);
                    sb.Append(heading6Text);
                    continue;
                
                case Paragraph p:
                    var paragraphRenderer = new Renderers.ParagraphRenderer();
                    var paragraphText = await paragraphRenderer.RenderAsync(p);
                    sb.Append(paragraphText);
                    continue;

                case Hyperlink hl:
                    var hyperlinkRenderer = new HyperlinkRenderer();
                    var hyperlinkText = await hyperlinkRenderer.RenderAsync(hl);
                    sb.Append(hyperlinkText);
                    continue;
                
                case List l:
                    var unorderedListRenderer = new UnorderedListRenderer();
                    var unorderedListText = await unorderedListRenderer.RenderAsync(l);
                    sb.Append(unorderedListText);
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
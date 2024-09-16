using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.Content.Extensions;

public static class HtmlRendererExtensions
{
    public static HtmlRenderer AddCommonRenderers(this HtmlRenderer htmlRenderer)
    {
        htmlRenderer.AddRenderer(new HyperlinkRenderer { Order = 10 });
        htmlRenderer.AddRenderer(new Heading1Renderer { Order = 11 });
        htmlRenderer.AddRenderer(new Heading2Renderer { Order = 12 });
        htmlRenderer.AddRenderer(new Heading3Renderer { Order = 13 });
        htmlRenderer.AddRenderer(new Heading4Renderer { Order = 14 });
        htmlRenderer.AddRenderer(new Heading5Renderer { Order = 15 });
        htmlRenderer.AddRenderer(new Heading6Renderer { Order = 16 });
        htmlRenderer.AddRenderer(new ParagraphRenderer { Order = 17 });
        htmlRenderer.AddRenderer(new ExternalNavigationLinkRenderer { Order = 18 });
        htmlRenderer.AddRenderer(new MailtoLinkRenderer { Order = 19 });
        
        return htmlRenderer;
    }
}
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

/// <summary>
///     A renderer to render out a contentful rich text field as the required HTML for a GovUK success banner.
/// </summary>
public class SuccessBannerRenderer : HtmlModelRendererBase, ISuccessBannerRenderer
{
    public SuccessBannerRenderer()
    {
        Renderer.AddRenderer(new SuccessBannerParagraphRenderer { Order = 1 });
        Renderer.AddRenderer(new HyperlinkRenderer { Order = 2 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        return await GetHtml(content);
    }
}
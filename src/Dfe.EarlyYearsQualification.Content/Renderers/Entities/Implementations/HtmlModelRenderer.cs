using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

/// <summary>
///     A standard HTML renderer for Contentful content.
/// </summary>
public class HtmlModelRenderer : HtmlModelRendererBase, IHtmlRenderer
{
    public HtmlModelRenderer()
    {
        Renderer.AddCommonRenderers()
                .AddRenderer(new UnorderedListRenderer { Order = 19 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        return await GetHtml(content);
    }
}
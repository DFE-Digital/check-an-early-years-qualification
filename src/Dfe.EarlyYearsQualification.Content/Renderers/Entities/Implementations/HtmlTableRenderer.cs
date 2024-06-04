using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

/// <summary>
///     A renderer to render out contentful tables as HTML tables + any content within.
/// </summary>
public class HtmlTableRenderer : HtmlModelRendererBase, IHtmlTableRenderer
{
    public HtmlTableRenderer()
    {
        Renderer.AddCommonRenderers()
                .AddRenderer(new UnorderedListRenderer { Order = 18 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        return await GetHtml(content);
    }
}
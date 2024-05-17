using Contentful.Core;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

/// <summary>
///     A HTML renderer to turn a contentful document into a GovUK Inset text HTML element.
/// </summary>
public class GovUkInsetTextRenderer : HtmlModelRendererBase, IGovUkInsetTextRenderer
{
    public GovUkInsetTextRenderer(IContentfulClient client)
    {
        Renderer.AddCommonRenderers()
                .AddRenderer(new InsetTextRenderer(client) { Order = 18 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        return await GetHtml(content);
    }
}
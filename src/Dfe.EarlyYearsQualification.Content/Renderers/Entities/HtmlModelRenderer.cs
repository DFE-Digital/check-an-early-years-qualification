using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities;

/// <summary>
///     A standard HTML renderer for Contentful content.
/// </summary>
public class HtmlModelRenderer : HtmlModelRendererBase, IHtmlRenderer
{
    public HtmlModelRenderer()
    {
        Renderer.AddRenderer(new UnorderedListRenderer { Order = 18 });
    }

    /// <summary>
    ///     Returns a string representation (HTML) of the <paramref name="content" />
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<string> ToHtml(Document? content)
    {
        if (content is null)
        {
            return string.Empty;
        }

        return await Renderer.ToHtml(content);
    }
}
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public abstract class HtmlModelRendererBase
{
    protected readonly HtmlRenderer Renderer = new();

    /// <summary>
    ///     Returns a string representation (HTML) of the <paramref name="content" />
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    protected async Task<string> GetHtml(Document? content)
    {
        if (content is null)
        {
            return string.Empty;
        }

        return await Renderer.ToHtml(content);
    }
}
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public abstract class HtmlModelRendererBase
{
    protected readonly HtmlRenderer Renderer = new();

    protected HtmlModelRendererBase()
    {
        Renderer.AddCommonRenderers();
    }
}
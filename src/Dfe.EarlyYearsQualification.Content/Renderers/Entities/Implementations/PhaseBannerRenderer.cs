using Document = Contentful.Core.Models.Document;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

public class PhaseBannerRenderer : HtmlModelRendererBase, IPhaseBannerRenderer
{
    public PhaseBannerRenderer()
    {
        Renderer.AddRenderer(new GovUk.PhaseBannerRenderer { Order = 1 });
    }

    public Task<string> ToHtml(Document? content)
    {
        return GetHtml(content);
    }
}
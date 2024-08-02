using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Entities.Implementations;

public class SideContentRenderer : HtmlModelRendererBase, ISideContentRenderer
{
    public SideContentRenderer()
    {
        Renderer.AddCommonRenderers()
                .AddRenderer(new UnorderedListHyperlinksRenderer { Order = 19 });
    }

    public Task<string> ToHtml(Document? content)
    {
        return GetHtml(content);
    }
}
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using TableRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableRenderer;
using TableRowRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableRowRenderer;
using TableHeadingRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableHeadingRenderer;
using TableCellRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableCellRenderer;

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
        
        Renderer.AddRenderer(new TableRenderer { Order = 19 });
        Renderer.AddRenderer(new TableRowRenderer { Order = 20 });
        Renderer.AddRenderer(new TableHeadingRenderer { Order = 21 });
        Renderer.AddRenderer(new TableCellRenderer { Order = 22 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        return await GetHtml(content);
    }
}
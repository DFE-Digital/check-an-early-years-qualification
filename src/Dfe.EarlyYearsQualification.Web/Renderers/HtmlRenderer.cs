using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

namespace Dfe.EarlyYearsQualification.Web.Renderers;

public class HtmlRenderer : IHtmlRenderer
{
    private readonly Contentful.Core.Models.HtmlRenderer _renderer = new();

    public HtmlRenderer()
    {
        _renderer.AddCommonRenderers()
                 .AddRenderer(new UnorderedListRenderer { Order = 18 });
    }

    public async Task<string> ToHtml(Document? content)
    {
        if (content is null)
        {
            return string.Empty;
        }

        return await _renderer.ToHtml(content);
    }
}
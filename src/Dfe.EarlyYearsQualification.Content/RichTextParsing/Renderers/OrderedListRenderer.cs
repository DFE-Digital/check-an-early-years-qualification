using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class OrderedListRenderer : BaseListRenderer, IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        return await RenderAsync(content, ListRendererType.OrderedList);
    }

    public bool SupportsContent(IContent content)
    {
        return content is List { NodeType: "ordered-list" };
    }
}
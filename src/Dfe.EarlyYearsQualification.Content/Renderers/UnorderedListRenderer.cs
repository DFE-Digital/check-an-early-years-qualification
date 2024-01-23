using System.Text;
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class UnorderedListRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var list = content as List;
        var sb = new StringBuilder();
        sb.Append("<ul class=\"govuk-list govuk-list--bullet\">");
        foreach (IContent listItem in list!.Content)
        {
            var listItemValue = listItem as ListItem;
            var listItemParagraph = listItemValue!.Content[0] as Paragraph;
            var listItemText = listItemParagraph!.Content[0] as Text;
            sb.Append($"<li>{listItemText!.Value}</li>");
        }
        sb.Append("</ul>");
        return Task.FromResult(sb.ToString());
    }

    public bool SupportsContent(IContent content)
    {
        return content is List && (content as List)!.NodeType == "unordered-list";
    }
}

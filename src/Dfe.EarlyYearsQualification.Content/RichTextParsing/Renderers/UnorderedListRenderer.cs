using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class UnorderedListRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        var list = content as List;

        if (list!.Content.Count == 0)
        {
            return await Task.FromResult(string.Empty);
        }

        var sb = new StringBuilder();
        sb.Append("<ul class=\"govuk-list govuk-list--bullet\">");

        foreach (var contentValue in list.Content)
        {
            if (contentValue is not ListItem listItem)
            {
                continue;
            }

            var listItemParagraph = listItem.Content[0] as Paragraph;
            sb.Append($"<li>{await NestedContentHelper.Render(listItemParagraph!.Content)}</li>");
        }

        sb.Append("</ul>");
        return await Task.FromResult(sb.ToString());
    }

    public bool SupportsContent(IContent content)
    {
        return content is List { NodeType: "unordered-list" };
    }
}
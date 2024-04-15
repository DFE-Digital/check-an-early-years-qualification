using System.Text;
using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers;

public class UnorderedListHyperlinksRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var list = content as List;
        var sb = new StringBuilder();
        sb.Append("<ul class=\"govuk-list govuk-list--spaced govuk-!-font-size-16\">");
        foreach (var listItem in list!.Content)
        {
            if (listItem is ListItem listItemValue)
            {
                var paragraph = listItemValue.Content[0] as Paragraph;
                var hyperlink = paragraph!.Content.First(x => x.GetType() == typeof(Hyperlink)) as Hyperlink;
                var hyperlinkText = hyperlink!.Content[0] as Text;
                sb.Append($"<li><a href='{hyperlink.Data.Uri}' class='govuk-link'>{hyperlinkText!.Value}</a></li>");
            }
        }
        sb.Append("</ul>");
        return Task.FromResult(sb.ToString());
    }

    public bool SupportsContent(IContent content)
    {
        return content is List { NodeType: "unordered-list" };
    }
}

using System.Security.Cryptography.X509Certificates;
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
        foreach (IContent listItem in list!.Content)
        {
            var listItemValue = listItem as ListItem;
            var paragraph = listItemValue!.Content[0] as Paragraph;
            var hyperlink = paragraph!.Content.Where(x => x.GetType() == typeof(Hyperlink)).First() as Hyperlink;
            var hyperlinkText = hyperlink!.Content[0] as Text;
            sb.Append($"<li><a href='{hyperlink!.Data.Uri}' class='govuk-link'>{hyperlinkText!.Value}</a></li>");
        }
        sb.Append("</ul>");
        return Task.FromResult(sb.ToString());
    }

    public bool SupportsContent(IContent content)
    {
        return content is List && (content as List)!.NodeType == "unordered-list";
    }
}

using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

public class TableRowRenderer : IContentRenderer
{
    public int Order { get; set; }

    public async Task<string> RenderAsync(IContent content)
    {
        var row = content as TableRow;
        var sb = new StringBuilder();

        sb.Append("<tr class='govuk-table__row'>");
        sb.Append(await TableHelper.Render(row!.Content));
        sb.Append("</tr>");

        return sb.ToString();
    }

    public bool SupportsContent(IContent content)
    {
        return content is TableRow;
    }
}
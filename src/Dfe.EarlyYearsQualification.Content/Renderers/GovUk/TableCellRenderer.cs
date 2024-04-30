using Contentful.Core.Models;

namespace Dfe.EarlyYearsQualification.Content.Renderers.GovUk;

public class TableCellRenderer : IContentRenderer
{
    public int Order { get; set; }

    public Task<string> RenderAsync(IContent content)
    {
        var heading = content as TableCell;
        var headingParagraph = heading!.Content[0] as Paragraph;
        var headingText = headingParagraph!.Content[0] as Text;
        return Task.FromResult("<td class='govuk-table__cell'>" + headingText!.Value + "</td>");
    }

    public bool SupportsContent(IContent content)
    {
        return content is TableCell;
    }
}
using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

public static class TableHelper
{
    public static async Task<string> Render(List<IContent> content)
    {
        var sb = new StringBuilder();

        foreach (var item in content)
        {
            switch (item)
            {
                case TableRow tr:
                    var tableRowRenderer = new Renderers.TableRowRenderer();
                    var tableRowContent = await tableRowRenderer.RenderAsync(tr);
                    sb.Append(tableRowContent);
                    continue;

                case TableHeader th:
                    var tableHeaderRenderer = new TableHeadingRenderer();
                    var tableHeadingText = await tableHeaderRenderer.RenderAsync(th);
                    sb.Append(tableHeadingText);
                    continue;

                case TableCell tc:
                    var tableCellRenderer = new Renderers.TableCellRenderer();
                    var tableCellText = await tableCellRenderer.RenderAsync(tc);
                    sb.Append(tableCellText);
                    continue;
            }
        }

        return sb.ToString();
    }
}
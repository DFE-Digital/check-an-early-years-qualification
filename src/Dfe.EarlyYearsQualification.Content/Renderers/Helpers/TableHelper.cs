using System.Text;
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using TableCellRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableCellRenderer;
using TableRowRenderer = Dfe.EarlyYearsQualification.Content.Renderers.GovUk.TableRowRenderer;

namespace Dfe.EarlyYearsQualification.Content.Renderers.Helpers;

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
                    var tableRowRenderer = new TableRowRenderer();
                    var tableRowContent = await tableRowRenderer.RenderAsync(tr);
                    sb.Append(tableRowContent);
                    continue;

                case TableHeader th:
                    var tableHeaderRenderer = new TableHeadingRenderer();
                    var tableHeadingText = await tableHeaderRenderer.RenderAsync(th);
                    sb.Append(tableHeadingText);
                    continue;

                case TableCell tc:
                    var tableCellRenderer = new TableCellRenderer();
                    var tableCellText = await tableCellRenderer.RenderAsync(tc);
                    sb.Append(tableCellText);
                    continue;
            }
        }

        return sb.ToString();
    }
}
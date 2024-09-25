using Contentful.Core.Models;
using FluentAssertions;
using TableCellRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableCellRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class TableCellRendererTests
{
    [TestMethod]
    public void TableCellRenderer_SupportsTableCell()
    {
        var cell = new TableCell();
        new TableCellRenderer().SupportsContent(cell).Should().BeTrue();
    }

    [TestMethod]
    public void TableCellRenderer_DoesNotSupportTableRow()
    {
        var tableRow = new TableRow();
        new TableCellRenderer().SupportsContent(tableRow).Should().BeFalse();
    }

    [TestMethod]
    public void TableCellRenderer_DoesNotSupportTableHeading()
    {
        var tableHeader = new TableHeader();
        new TableCellRenderer().SupportsContent(tableHeader).Should().BeFalse();
    }

    [TestMethod]
    public void TableCellRenderer_DoesNotSupportTable()
    {
        var table = new Table();
        new TableCellRenderer().SupportsContent(table).Should().BeFalse();
    }

    [TestMethod]
    public void TableCellRenderer_RendersTableCell()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };

        var cell = new TableCell { Content = [para] };

        var renderer = new TableCellRenderer();
        var result = renderer.RenderAsync(cell).Result;

        result.Should().Be("<td class='govuk-table__cell'>Some text.</td>");
    }
}
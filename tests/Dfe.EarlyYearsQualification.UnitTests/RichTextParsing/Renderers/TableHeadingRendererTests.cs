using Contentful.Core.Models;
using TableHeadingRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableHeadingRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class TableHeadingRendererTests
{
    [TestMethod]
    public void TableHeadingRenderer_SupportsTableHeading()
    {
        var tableHeader = new TableHeader();
        new TableHeadingRenderer().SupportsContent(tableHeader).Should().BeTrue();
    }

    [TestMethod]
    public void TableHeadingRenderer_DoesNotSupportTableRow()
    {
        var tableRow = new TableRow();
        new TableHeadingRenderer().SupportsContent(tableRow).Should().BeFalse();
    }

    [TestMethod]
    public void TableHeadingRenderer_DoesNotSupportTableCell()
    {
        var tableCell = new TableCell();
        new TableHeadingRenderer().SupportsContent(tableCell).Should().BeFalse();
    }

    [TestMethod]
    public void TableHeadingRenderer_DoesNotSupportTable()
    {
        var table = new Table();
        new TableHeadingRenderer().SupportsContent(table).Should().BeFalse();
    }

    [TestMethod]
    public void TableHeadingRenderer_RendersTableHeading()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };

        var cell = new TableHeader { Content = [para] };

        var renderer = new TableHeadingRenderer();
        var result = renderer.RenderAsync(cell).Result;

        result.Should().Be("<th class='govuk-table__header'>Some text.</th>");
    }
}
using Contentful.Core.Models;
using FluentAssertions;
using TableRowRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRowRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class TableRowRendererTests
{
    [TestMethod]
    public void TableRowRenderer_SupportsTableRow()
    {
        var row = new TableRow();
        new TableRowRenderer().SupportsContent(row).Should().BeTrue();
    }

    [TestMethod]
    public void TableRowRenderer_DoesNotSupportTableCell()
    {
        var cell = new TableCell();
        new TableRowRenderer().SupportsContent(cell).Should().BeFalse();
    }

    [TestMethod]
    public void TableRowRenderer_DoesNotSupportTableHeading()
    {
        var tableHeader = new TableHeader();
        new TableRowRenderer().SupportsContent(tableHeader).Should().BeFalse();
    }

    [TestMethod]
    public void TableRowRenderer_DoesNotSupportTable()
    {
        var table = new Table();
        new TableRowRenderer().SupportsContent(table).Should().BeFalse();
    }

    [TestMethod]
    public void TableRowRenderer_RendersTableRowWithNestedContent()
    {
        var row =
            new TableRow
            {
                Content =
                [
                    new TableHeader
                    {
                        Content =
                        [
                            new Paragraph
                            {
                                Content =
                                [
                                    new Text { Value = "Heading" }
                                ]
                            }
                        ]
                    },
                    new TableCell
                    {
                        Content =
                        [
                            new Paragraph
                            {
                                Content =
                                [
                                    new Text { Value = "Cell" }
                                ]
                            }
                        ]
                    }
                ]
            };

        var renderer = new TableRowRenderer();
        var result = renderer.RenderAsync(row).Result;

        result.Should().Be("<tr class='govuk-table__row'>" +
                           "<th class='govuk-table__header'>Heading</th>" +
                           "<td class='govuk-table__cell'>Cell</td>" +
                           "</tr>");
    }
}
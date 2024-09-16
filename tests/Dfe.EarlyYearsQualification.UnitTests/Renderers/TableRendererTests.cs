using Contentful.Core.Models;
using FluentAssertions;
using TableRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.TableRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class TableRendererTests
{
    [TestMethod]
    public void TableRenderer_SupportsTable()
    {
        var table = new Table();
        new TableRenderer().SupportsContent(table).Should().BeTrue();
    }

    [TestMethod]
    public void TableRenderer_DoesNotSupportTableRow()
    {
        var tableRow = new TableRow();
        new TableRenderer().SupportsContent(tableRow).Should().BeFalse();
    }

    [TestMethod]
    public void TableRenderer_DoesNotSupportTableCell()
    {
        var tableCell = new TableCell();
        new TableRenderer().SupportsContent(tableCell).Should().BeFalse();
    }

    [TestMethod]
    public void TableRenderer_DoesNotSupportTableHeading()
    {
        var tableHeader = new TableHeader();
        new TableRenderer().SupportsContent(tableHeader).Should().BeFalse();
    }

    [TestMethod]
    public void TableHeadingRenderer_RendersTableWithNestedContent()
    {
        var table
            = new Table
              {
                  Content =
                  [
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
                                              new Text
                                              {
                                                  Value = "Heading 1"
                                              }
                                          ]
                                      }
                                  ]
                              },
                              new TableHeader
                              {
                                  Content =
                                  [
                                      new Paragraph
                                      {
                                          Content =
                                          [
                                              new Text
                                              {
                                                  Value = "Heading 2"
                                              }
                                          ]
                                      }
                                  ]
                              }
                          ]
                      },
                      new TableRow
                      {
                          Content =
                          [
                              new TableCell
                              {
                                  Content =
                                  [
                                      new Paragraph
                                      {
                                          Content =
                                          [
                                              new Text
                                              {
                                                  Value = "Cell 1"
                                              }
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
                                              new Text
                                              {
                                                  Value =
                                                      "Cell 2"
                                              }
                                          ]
                                      }
                                  ]
                              }
                          ]
                      }
                  ]
              };

        var renderer = new TableRenderer();
        var result = renderer.RenderAsync(table).Result;

        result.Should().Be("<table class='govuk-table'>" +
                           "<tr class='govuk-table__row'>" +
                           "<th class='govuk-table__header'>Heading 1</th>" +
                           "<th class='govuk-table__header'>Heading 2</th>" +
                           "</tr>" +
                           "<tr class='govuk-table__row'>" +
                           "<td class='govuk-table__cell'>Cell 1</td>" +
                           "<td class='govuk-table__cell'>Cell 2</td>" +
                           "</tr>" +
                           "</table>");
    }
}
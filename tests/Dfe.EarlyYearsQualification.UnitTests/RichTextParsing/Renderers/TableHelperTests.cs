using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Helpers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class TableHelperTests
{
    private readonly TableCell _tableCell
        = new()
          {
              Content =
              [
                  new Paragraph
                  {
                      Content =
                      [
                          new Text
                          {
                              Value = "Test"
                          }
                      ]
                  }
              ]
          };

    private readonly TableHeader _tableHeader
        = new()
          {
              Content =
              [
                  new Paragraph
                  {
                      Content =
                      [
                          new Text
                          {
                              Value = "Test"
                          }
                      ]
                  }
              ]
          };

    private readonly TableRow _tableRow
        = new()
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
                                      Value =
                                          "Test"
                                  }
                              ]
                          }
                      ]
                  }
              ]
          };

    [TestMethod]
    public void TableHelper_RendersTableRow()
    {
        var content = new List<IContent> { _tableRow };

        var output = TableHelper.Render(content).Result;

        output.Should().Be("<tr class='govuk-table__row'><td class='govuk-table__cell'>Test</td></tr>");
    }

    [TestMethod]
    public void TableHelper_RendersTableHeader()
    {
        var content = new List<IContent> { _tableHeader };

        var output = TableHelper.Render(content).Result;

        output.Should().Be("<th class='govuk-table__header'>Test</th>");
    }

    [TestMethod]
    public void TableHelper_RendersTableCell()
    {
        var content = new List<IContent> { _tableCell };

        var output = TableHelper.Render(content).Result;

        output.Should().Be("<td class='govuk-table__cell'>Test</td>");
    }

    [TestMethod]
    public void TableHelper_RendersMultipleSupported()
    {
        var content = new List<IContent> { _tableCell, _tableHeader, _tableRow };

        var output = TableHelper.Render(content).Result;

        output.Should()
              .Be("<td class='govuk-table__cell'>Test</td><th class='govuk-table__header'>Test</th><tr class='govuk-table__row'><td class='govuk-table__cell'>Test</td></tr>");
    }
}
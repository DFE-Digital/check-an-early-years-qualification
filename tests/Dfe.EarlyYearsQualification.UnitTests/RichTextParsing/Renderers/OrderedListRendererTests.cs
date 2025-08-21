using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class OrderedListRendererTests
{
   [TestMethod]
    public void OrderedListRenderer_SupportsOrderedList()
    {
        var list = new List { NodeType = "ordered-list" };

        new OrderedListRenderer().SupportsContent(list).Should().BeTrue();
    }

    [TestMethod]
    public void OrderedListRenderer_DoesNotSupportAnotherList()
    {
        var list = new List { NodeType = "unordered-list" };

        new OrderedListRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void OrderedListRenderer_DoesNotSupportHyperlink()
    {
        var link = new Hyperlink();

        new OrderedListRenderer().SupportsContent(link).Should().BeFalse();
    }

    [TestMethod]
    public void OrderedListRenderer_RendersEmptyListAsNothing()
    {
        var list = new List { NodeType = "ordered-list", Content = [] };

        var orderedListRenderer = new OrderedListRenderer();
        var result = orderedListRenderer.RenderAsync(list).Result;

        result.Should().Be("");
    }

    [TestMethod]
    public void OrderedListRenderer_RendersListWithOneParagraph()
    {
        var para = new Paragraph { Content = [new Text { Value = "Paragraph text." }] };

        var listItem = new ListItem { Content = [para] };

        var list = new List { NodeType = "ordered-list", Content = [listItem] };

        var renderer = new OrderedListRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should().Be("<ol class=\"govuk-list govuk-list--number\"><li>Paragraph text.</li></ol>");
    }

    [TestMethod]
    public void OrderedListRenderer_RendersListWithTwoItems()
    {
        var para1 = new Paragraph { Content = [new Text { Value = "First paragraph text." }] };

        var listItem1 = new ListItem { Content = [para1] };

        var para2 = new Paragraph { Content = [new Text { Value = "Second paragraph text." }] };

        var listItem2 = new ListItem { Content = [para2] };

        var list = new List { NodeType = "ordered-list", Content = [listItem1, listItem2] };

        var renderer = new OrderedListRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should()
              .Be("<ol class=\"govuk-list govuk-list--number\"><li>First paragraph text.</li><li>Second paragraph text.</li></ol>");
    }
}
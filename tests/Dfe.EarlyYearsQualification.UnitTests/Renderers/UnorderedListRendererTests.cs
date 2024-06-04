using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class UnorderedListRendererTests
{
    [TestMethod]
    public void UnorderedListRenderer_SupportsUnorderedList()
    {
        var list = new List { NodeType = "unordered-list" };

        new UnorderedListRenderer().SupportsContent(list).Should().BeTrue();
    }

    [TestMethod]
    public void UnorderedListRenderer_DoesNotSupportAnotherList()
    {
        var list = new List { NodeType = "ordered-list" };

        new UnorderedListRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void UnorderedListRenderer_DoesNotSupportHyperlink()
    {
        var list = new Hyperlink();

        new UnorderedListRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void UnorderedListRenderer_RendersEmptyListAsNothing()
    {
        var list = new List { NodeType = "unordered-list", Content = [] };

        var unorderedListRenderer = new UnorderedListRenderer();
        var result = unorderedListRenderer.RenderAsync(list).Result;

        result.Should().Be("");
    }

    [TestMethod]
    public void UnorderedListRenderer_RendersListWithOneParagraph()
    {
        var para = new Paragraph { Content = [new Text { Value = "Paragraph text." }] };

        var listItem = new ListItem { Content = [para] };

        var list = new List { NodeType = "unordered-list", Content = [listItem] };

        var renderer = new UnorderedListRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should().Be("<ul class=\"govuk-list govuk-list--bullet\"><li>Paragraph text.</li></ul>");
    }

    [TestMethod]
    public void UnorderedListRenderer_RendersListWithTwoItems()
    {
        var para1 = new Paragraph { Content = [new Text { Value = "First paragraph text." }] };

        var listItem1 = new ListItem { Content = [para1] };

        var para2 = new Paragraph { Content = [new Text { Value = "Second paragraph text." }] };

        var listItem2 = new ListItem { Content = [para2] };

        var list = new List { NodeType = "unordered-list", Content = [listItem1, listItem2] };

        var renderer = new UnorderedListRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should()
              .Be("<ul class=\"govuk-list govuk-list--bullet\"><li>First paragraph text.</li><li>Second paragraph text.</li></ul>");
    }
}
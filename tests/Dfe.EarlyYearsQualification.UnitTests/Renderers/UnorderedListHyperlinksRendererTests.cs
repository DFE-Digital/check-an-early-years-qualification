using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class UnorderedListHyperlinksRendererTests
{
    [TestMethod]
    public void UnorderedListHyperlinksRenderer_SupportsUnorderedList()
    {
        var list = new List { NodeType = "unordered-list" };

        new UnorderedListHyperlinksRenderer().SupportsContent(list).Should().BeTrue();
    }

    [TestMethod]
    public void UnorderedListHyperlinksRenderer_DoesNotSupportAnotherList()
    {
        var list = new List { NodeType = "ordered-list" };

        new UnorderedListHyperlinksRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void UnorderedListHyperlinksRenderer_DoesNotSupportHyperlink()
    {
        var list = new Hyperlink();

        new UnorderedListHyperlinksRenderer().SupportsContent(list).Should().BeFalse();
    }

    [TestMethod]
    public void UnorderedListHyperlinksRenderer_RendersEmptyListAsNothing()
    {
        var list = new List { NodeType = "unordered-list", Content = [] };

        var unorderedListRenderer = new UnorderedListHyperlinksRenderer();
        var result = unorderedListRenderer.RenderAsync(list).Result;

        result.Should().Be("");
    }

    [TestMethod]
    public void UnorderedListHyperlinksRenderer_RendersListWithOneItem()
    {
        var hyperlink = new Hyperlink
                        {
                            Content = [new Text { Value = "Hyperlink1" }],
                            Data = new HyperlinkData { Uri = "https://site.one.com" }
                        };

        var paragraph = new Paragraph { Content = [hyperlink] };

        var listItem = new ListItem { Content = [paragraph] };

        var list = new List { NodeType = "unordered-list", Content = [listItem] };

        var renderer = new UnorderedListHyperlinksRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should()
              .Be("<ul class=\"govuk-list govuk-list--spaced govuk-!-font-size-16\"><li><a href='https://site.one.com' class='govuk-link'>Hyperlink1</a></li></ul>");
    }

    [TestMethod]
    public void UnorderedListHyperlinksRenderer_RendersListWithTwoItems()
    {
        var hyperlink1 = new Hyperlink
                         {
                             Content = [new Text { Value = "Hyperlink1" }],
                             Data = new HyperlinkData { Uri = "https://site.one.com" }
                         };

        var para1 = new Paragraph { Content = [hyperlink1] };

        var listItem1 = new ListItem { Content = [para1] };

        var hyperlink2 = new Hyperlink
                         {
                             Content = [new Text { Value = "Hyperlink2" }],
                             Data = new HyperlinkData { Uri = "https://site.two.com" }
                         };

        var para2 = new Paragraph { Content = [hyperlink2] };

        var listItem2 = new ListItem { Content = [para2] };

        var list = new List { NodeType = "unordered-list", Content = [listItem1, listItem2] };

        var renderer = new UnorderedListHyperlinksRenderer();
        var result = renderer.RenderAsync(list).Result;

        result.Should()
              .Be("<ul class=\"govuk-list govuk-list--spaced govuk-!-font-size-16\"><li><a href='https://site.one.com' class='govuk-link'>Hyperlink1</a></li><li><a href='https://site.two.com' class='govuk-link'>Hyperlink2</a></li></ul>");
    }
}
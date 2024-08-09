using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.Helpers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class NestedContentHelperTests
{
    [TestMethod]
    public void ContentHelper_RenderParagraph()
    {
        var paragraph = new Paragraph { Content = [] };

        var text = new Text { Value = "Text" };

        paragraph.Content.Add(text);

        var content = new List<IContent> { paragraph };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<p class=\"govuk-body\">Text</p>");
    }

    [TestMethod]
    public void ContentHelper_RenderHyperlink()
    {
        var hyperlink = new Hyperlink
                        {
                            Content = [],
                            Data = new HyperlinkData
                                   {
                                       Uri = "https://my.uri"
                                   }
                        };

        var text = new Text { Value = "Hyperlink Content" };

        hyperlink.Content.Add(text);

        var content = new List<IContent> { hyperlink };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<a href='https://my.uri' class='govuk-link'>Hyperlink Content</a>");
    }

    [TestMethod]
    public void ContentHelper_RenderText()
    {
        var text = new Text { Value = "Some text" };

        var content = new List<IContent> { text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("Some text");
    }
    
    [TestMethod]
    public void ContentHelper_RenderBoldText()
    {
        var text = new Text { Value = "Some text", Marks = [ new Mark
                                                             {
                                                                 Type = "bold"
                                                             }]};

        var content = new List<IContent> { text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should().Be("<b>Some text</b>");
    }

    [TestMethod]
    public void ContentHelper_RenderCollection()
    {
        var paragraph = new Paragraph { Content = [] };

        var paraText = new Text { Value = "Text" };

        paragraph.Content.Add(paraText);

        var hyperlink = new Hyperlink
                        {
                            Content = [],
                            Data = new HyperlinkData
                                   {
                                       Uri = "https://my.uri"
                                   }
                        };

        var hlText = new Text { Value = "Hyperlink Content" };

        hyperlink.Content.Add(hlText);

        var text = new Text { Value = "Some text" };

        var content = new List<IContent> { paragraph, hyperlink, text };

        var output = NestedContentHelper.Render(content).Result;

        output.Should()
              .Be("<p class=\"govuk-body\">Text</p><a href='https://my.uri' class='govuk-link'>Hyperlink Content</a>Some text");
    }
}
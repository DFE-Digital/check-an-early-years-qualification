using Contentful.Core.Models;
using FluentAssertions;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class ParagraphRendererTests
{
    [TestMethod]
    public void GovUkParagraphRenderer_SupportsPara()
    {
        var para = new Paragraph();
        new ParagraphRenderer().SupportsContent(para).Should().BeTrue();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportText()
    {
        var para = new Text();
        new ParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportHyperlink()
    {
        var para = new Hyperlink();
        new ParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_RendersPara()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };

        var renderer = new ParagraphRenderer();
        var result = renderer.RenderAsync(para).Result;

        result.Should().Be("<p class=\"govuk-body\">Some text.</p>");
    }
}
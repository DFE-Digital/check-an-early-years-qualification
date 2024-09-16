using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class SuccessBannerParagraphRendererTests
{
    [TestMethod]
    public void GovUkParagraphRenderer_SupportsPara()
    {
        var para = new Paragraph();
        new SuccessBannerParagraphRenderer().SupportsContent(para).Should().BeTrue();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportText()
    {
        var para = new Text();
        new SuccessBannerParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportHyperlink()
    {
        var para = new Hyperlink();
        new SuccessBannerParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_RendersTextOnly()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };

        var renderer = new SuccessBannerParagraphRenderer();
        var result = renderer.RenderAsync(para).Result;

        result.Should().Be("Some text.");
    }
}
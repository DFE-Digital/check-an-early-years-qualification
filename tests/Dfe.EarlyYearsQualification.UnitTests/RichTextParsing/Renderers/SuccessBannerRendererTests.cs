using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class SuccessBannerRendererTests
{
    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportsBasePara()
    {
        var para = new Paragraph();
        new SuccessBannerRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportText()
    {
        var para = new Text();
        new SuccessBannerRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportHyperlink()
    {
        var para = new Hyperlink();
        new SuccessBannerRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_SupportsParaWithCustomNodeType()
    {
        var para = new Paragraph
                   {
                       NodeType = "SuccessBannerContent"
                   };
        new SuccessBannerRenderer().SupportsContent(para).Should().BeTrue();
    }

    [TestMethod]
    public void GovUkParagraphRenderer_RendersTextOnly()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };

        var renderer = new SuccessBannerRenderer();
        var result = renderer.RenderAsync(para).Result;

        result.Should().Be("Some text.");
    }
}
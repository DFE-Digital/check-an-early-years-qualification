using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class Heading3RendererTests
{
    [TestMethod]
    public void Heading3Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading3Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading3Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading3Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading3Renderer_DoesNotSupportHeading1()
    {
        var heading = new Heading1();
        new Heading3Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading3Renderer_SupportsHeading3()
    {
        var heading = new Heading3();
        new Heading3Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading3Renderer_RendersHeading()
    {
        var heading = new Heading3 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading3Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h3 class='govuk-heading-m'>Heading text</h3>");
    }
}
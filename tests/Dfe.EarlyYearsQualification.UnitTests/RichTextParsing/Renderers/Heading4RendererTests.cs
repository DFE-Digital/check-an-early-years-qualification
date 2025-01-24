using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class Heading4RendererTests
{
    [TestMethod]
    public void Heading4Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading4Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading4Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading4Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading4Renderer_DoesNotSupportHeading1()
    {
        var heading = new Heading1();
        new Heading4Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading4Renderer_SupportsHeading4()
    {
        var heading = new Heading4();
        new Heading4Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading4Renderer_RendersHeading()
    {
        var heading = new Heading4 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading4Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h4 class='govuk-heading-s'>Heading text</h4>");
    }
}
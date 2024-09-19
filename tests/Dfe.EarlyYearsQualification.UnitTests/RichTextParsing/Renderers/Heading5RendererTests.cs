using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

[TestClass]
public class Heading5RendererTests
{
    [TestMethod]
    public void Heading5Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading5Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading5Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading5Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading5Renderer_DoesNotSupportHeading1()
    {
        var heading = new Heading1();
        new Heading5Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading5Renderer_SupportsHeading5()
    {
        var heading = new Heading5();
        new Heading5Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading5Renderer_RendersHeading()
    {
        var heading = new Heading5 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading5Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h5 class='govuk-heading-m'>Heading text</h5>");
    }
}
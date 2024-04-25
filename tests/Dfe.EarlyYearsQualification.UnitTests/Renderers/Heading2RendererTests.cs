using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class Heading2RendererTests
{
    [TestMethod]
    public void Heading2Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading2Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading2Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading2Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading2Renderer_DoesNotSupportHeading1()
    {
        var heading = new Heading1();
        new Heading2Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading2Renderer_SupportsHeading2()
    {
        var heading = new Heading2();
        new Heading2Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading2Renderer_RendersHeading()
    {
        var heading = new Heading2 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading2Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h2 class='govuk-heading-m'>Heading text</h2>");
    }
}
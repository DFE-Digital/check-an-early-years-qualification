using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class Heading1RendererTests
{
    [TestMethod]
    public void Heading1Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading1Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading1Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading1Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading1Renderer_DoesNotSupportHeading2()
    {
        var heading = new Heading2();
        new Heading1Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading1Renderer_SupportsHeading1()
    {
        var heading = new Heading1();
        new Heading1Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading1Renderer_RendersHeading()
    {
        var heading = new Heading1 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading1Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h1 class='govuk-heading-l'>Heading text</h1>");
    }
}
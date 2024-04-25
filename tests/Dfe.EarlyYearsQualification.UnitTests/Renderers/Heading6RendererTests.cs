using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class Heading6RendererTests
{
    [TestMethod]
    public void Heading6Renderer_DoesNotSupportPara()
    {
        var heading = new Paragraph();
        new Heading6Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading6Renderer_DoesNotSupportText()
    {
        var heading = new Text();
        new Heading6Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading6Renderer_DoesNotSupportHeading1()
    {
        var heading = new Heading1();
        new Heading6Renderer().SupportsContent(heading).Should().BeFalse();
    }

    [TestMethod]
    public void Heading6Renderer_SupportsHeading6()
    {
        var heading = new Heading6();
        new Heading6Renderer().SupportsContent(heading).Should().BeTrue();
    }

    [TestMethod]
    public void Heading6Renderer_RendersHeading()
    {
        var heading = new Heading6 { Content = [new Text { Value = "Heading text" }] };

        var result = new Heading6Renderer().RenderAsync(heading).Result;

        result.Should().Be("<h6 class='govuk-heading-m'>Heading text</h6>");
    }
}
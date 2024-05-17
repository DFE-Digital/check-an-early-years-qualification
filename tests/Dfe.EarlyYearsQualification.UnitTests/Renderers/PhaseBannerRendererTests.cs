using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class PhaseBannerRendererTests
{
    [TestMethod]
    public void PhaseBannerRendererTests_SupportsPara()
    {
        var para = new Paragraph();
        new PhaseBannerRenderer().SupportsContent(para).Should().BeTrue();
    }

    [TestMethod]
    public void PhaseBannerRendererTests_DoesNotSupportText()
    {
        var para = new Text();
        new PhaseBannerRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void PhaseBannerRendererTests_DoesNotSupportHyperlink()
    {
        var para = new Hyperlink();
        new PhaseBannerRenderer().SupportsContent(para).Should().BeFalse();
    }

    [TestMethod]
    public void PhaseBannerRendererTests_RemovesPTags()
    {
        var para = new Paragraph
                   {
                       Content =
                       [
                           new Text { Value = "Some text." },
                           new Hyperlink
                           {
                               Content = [new Text { Value = "Some text." }],
                               Data = new HyperlinkData { Uri = "https://some.website.com" }
                           }
                       ]
                   };

        var renderer = new PhaseBannerRenderer();
        var result = renderer.RenderAsync(para).Result;

        result.Should().Be("Some text.<a href='https://some.website.com' class='govuk-link'>Some text.</a>");
    }
}
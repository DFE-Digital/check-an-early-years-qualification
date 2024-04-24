using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers;
using Dfe.EarlyYearsQualification.Content.Renderers.GovUk;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class HyperlinkRendererTests
{
    [TestMethod]
    public void HyperlinkRenderer_DoesNotSupportPara()
    {
        var hyperlink = new Paragraph();
        new HyperlinkRenderer().SupportsContent(hyperlink).Should().BeFalse();
    }

    [TestMethod]
    public void HyperlinkRenderer_DoesNotSupportText()
    {
        var hyperlink = new Text();
        new HyperlinkRenderer().SupportsContent(hyperlink).Should().BeFalse();
    }

    [TestMethod]
    public void HyperlinkRenderer_SupportsHyperlink()
    {
        var hyperlink = new Hyperlink();
        new HyperlinkRenderer().SupportsContent(hyperlink).Should().BeTrue();
    }

    [TestMethod]
    public void HyperlinkRenderer_RendersHyperlink()
    {
        var text = new Text { Value = "Hyperlink text" };

        var hyperlink = new Hyperlink
                        {
                            Content = [text],
                            Data = new HyperlinkData { Uri = "https://some.website.com" }
                        };

        var result = new HyperlinkRenderer().RenderAsync(hyperlink).Result;

        result.Should().Be("<a href='https://some.website.com' class='govuk-link'>Hyperlink text</a>");
    }
}
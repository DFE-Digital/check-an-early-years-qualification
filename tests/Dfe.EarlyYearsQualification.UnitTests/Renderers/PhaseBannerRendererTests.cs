using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;
using Dfe.EarlyYearsQualification.Mock.Helpers;
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
    public void PhaseBannerRendererTests_HandlesExternalNavigationLinks()
    {
        var content = ContentfulContentHelper.ParagraphWithEmbeddedLink("Some text", "Link text", "linkHref");
        
        var renderer = new PhaseBannerRenderer();
        var result = renderer.RenderAsync(content).Result;
        
        result.Should().Be("Some text<a href='linkHref' class='govuk-link'>Link text</a>");
    }

    [TestMethod]
    public void PhaseBannerRendererTests_HandlesHyperLink()
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
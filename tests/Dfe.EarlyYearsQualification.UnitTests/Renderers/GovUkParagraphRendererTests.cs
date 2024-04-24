using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Renderers;
using FluentAssertions;

namespace Dfe.EarlyYearsQualification.UnitTests.Renderers;

[TestClass]
public class GovUkParagraphRendererTests
{
    [TestMethod]
    public void GovUkParagraphRenderer_SupportsPara()
    {
        var para = new Paragraph();
        new GovUkParagraphRenderer().SupportsContent(para).Should().BeTrue();
    }
    
    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportText()
    {
        var para = new Text();
        new GovUkParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }
    
    [TestMethod]
    public void GovUkParagraphRenderer_DoesNotSupportHyperlink()
    {
        var para = new Hyperlink();
        new GovUkParagraphRenderer().SupportsContent(para).Should().BeFalse();
    }
    
    [TestMethod]
    public void GovUkParagraphRenderer_RendersPara()
    {
        var text = new Text { Value = "Some text." };

        var para = new Paragraph { Content = [text] };
        
        var renderer = new GovUkParagraphRenderer();
        var result = renderer.RenderAsync(para).Result;

        result.Should().Be("<p class=\"govuk-body\">Some text.</p>");
    }
}
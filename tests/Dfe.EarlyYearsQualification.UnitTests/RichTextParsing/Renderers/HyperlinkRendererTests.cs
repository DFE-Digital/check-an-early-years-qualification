using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing.Renderers;

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

    [TestMethod]
    public async Task HyperlinkRenderer_RendersBoldHyperlinkText()
    {
        var text = new Text { Value = "Hyperlink text", Marks = [new Mark { Type = "bold" }] };

        var hyperlink = new Hyperlink
                        {
                            Content = [text],
                            Data = new HyperlinkData { Uri = "https://some.website.com" }
                        };

        var result = await new HyperlinkRenderer().RenderAsync(hyperlink);

        result.Should().Be("<a href='https://some.website.com' class='govuk-link'><b>Hyperlink text</b></a>");
    }

    [TestMethod]
    public async Task HyperlinkRenderer_RendersAllTextNodes()
    {
        var hyperlink = new Hyperlink
                        {
                            Content =
                            [
                                new Text { Value = "Learn more and " },
                                new Text { Value = "sign up", Marks = [new Mark { Type = "bold" }] }
                            ],
                            Data = new HyperlinkData { Uri = "https://some.website.com" }
                        };

        var result = await new HyperlinkRenderer().RenderAsync(hyperlink);

        result.Should().Be("<a href='https://some.website.com' class='govuk-link'>Learn more and <b>sign up</b></a>");
    }
}

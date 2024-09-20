using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using FluentAssertions;
using ParagraphRenderer = Dfe.EarlyYearsQualification.Content.RichTextParsing.Renderers.ParagraphRenderer;

namespace Dfe.EarlyYearsQualification.UnitTests.RichTextParsing;

[TestClass]
public class GovUkContentParserTests
{
    [TestMethod]
    public async Task GovUkContentfulParser_ConstructedWithRenderer_NoContent_ReturnsEmptyString()
    {
        var renderers = new List<IContentRenderer>()
                        {
                            new ParagraphRenderer(),
                        };

        var govUkContentfulParser = new GovUkContentParser(renderers);

        var result = await govUkContentfulParser.ToHtml(null);
        result.Should().Be(string.Empty);
    }
    
    [TestMethod]
    public async Task GovUkContentfulParser_ConstructedWithRenderer_Content_ReturnsEmptyString()
    {
        var renderers = new List<IContentRenderer>()
                        {
                            new ParagraphRenderer(),
                        };

        var govUkContentfulParser = new GovUkContentParser(renderers);

        var result = await govUkContentfulParser.ToHtml(ContentfulContentHelper.Paragraph("TEST"));
        result.Should().Be("<p class=\"govuk-body\">TEST</p>");
    }
}
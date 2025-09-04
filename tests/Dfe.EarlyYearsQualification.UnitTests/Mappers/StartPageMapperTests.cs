using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class StartPageMapperTests
{
    [TestMethod]
    public async Task Map_PassInParameters_ReturnsModel()
    {
        const string preCtaButtonContentHtml = "Pre CTA Button text";
        const string postCtaButtonContentHtml = "Post CTA Button text";
        const string rightHandSideContentHtml = "Right hand side content";
        var startPage = new StartPage
                        {
                            Header = "Header",
                            CtaButtonText = "Button text",
                            RightHandSideContentHeader = "Right side content header",
                            PreCtaButtonContent = ContentfulContentHelper.Paragraph(preCtaButtonContentHtml),
                            PostCtaButtonContent = ContentfulContentHelper.Paragraph(postCtaButtonContentHtml),
                            RightHandSideContent = ContentfulContentHelper.Paragraph(rightHandSideContentHtml)
                        };

        var mockContentParser = new Mock<IGovUkContentParser>();
        mockContentParser.Setup(x => x.ToHtml(startPage.PreCtaButtonContent)).ReturnsAsync(preCtaButtonContentHtml);
        mockContentParser.Setup(x => x.ToHtml(startPage.PostCtaButtonContent)).ReturnsAsync(postCtaButtonContentHtml);
        mockContentParser.Setup(x => x.ToHtml(startPage.RightHandSideContent)).ReturnsAsync(rightHandSideContentHtml);
        
        var mapper = new StartPageMapper(mockContentParser.Object);
        var result = await mapper.Map(startPage);

        result.Should().NotBeNull();
        result.Header.Should().BeSameAs(startPage.Header);
        result.PreCtaButtonContent.Should().BeSameAs(preCtaButtonContentHtml);
        result.CtaButtonText.Should().BeSameAs(startPage.CtaButtonText);
        result.PostCtaButtonContent.Should().BeSameAs(postCtaButtonContentHtml);
        result.RightHandSideContentHeader.Should().BeSameAs(startPage.RightHandSideContentHeader);
        result.RightHandSideContent.Should().BeSameAs(rightHandSideContentHtml);
    }
}
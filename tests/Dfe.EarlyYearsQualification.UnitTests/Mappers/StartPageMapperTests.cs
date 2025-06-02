using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Mappers;

namespace Dfe.EarlyYearsQualification.UnitTests.Mappers;

[TestClass]
public class StartPageMapperTests
{
    [TestMethod]
    public void Map_PassInParameters_ReturnsModel()
    {
        const string preCtaButtonContentHtml = "Pre CTA Button text";
        const string postCtaButtonContentHtml = "Post CTA Button text";
        const string rightHandSideContentHtml = "Right hand side content";
        var startPage = new StartPage
                        {
                            Header = "Header",
                            CtaButtonText = "Button text",
                            RightHandSideContentHeader = "Right side content header"
                        };

        var result = StartPageMapper.Map(startPage, preCtaButtonContentHtml, postCtaButtonContentHtml,
                                         rightHandSideContentHtml);

        result.Should().NotBeNull();
        result.Header.Should().BeSameAs(startPage.Header);
        result.PreCtaButtonContent.Should().BeSameAs(preCtaButtonContentHtml);
        result.CtaButtonText.Should().BeSameAs(startPage.CtaButtonText);
        result.PostCtaButtonContent.Should().BeSameAs(postCtaButtonContentHtml);
        result.RightHandSideContentHeader.Should().BeSameAs(startPage.RightHandSideContentHeader);
        result.RightHandSideContent.Should().BeSameAs(rightHandSideContentHtml);
    }
}
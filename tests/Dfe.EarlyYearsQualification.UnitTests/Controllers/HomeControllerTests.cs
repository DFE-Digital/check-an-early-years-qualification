using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HomeControllerTests
{
    [TestMethod]
    public async Task Index_ContentServiceReturnsNoContent_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller = new HomeController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                            mockUserJourneyCookieService.Object);

        mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync((StartPage?)null);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyCritical("Start page content not found");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsContent_ReturnsStartPageModel()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller = new HomeController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                            mockUserJourneyCookieService.Object);

        const string postCtaContentText = "This is the post cta content";
        const string preCtaContentText = "This is the pre cta content";
        const string sideContentText = "This is the side content";

        var postCtaButtonContent = ContentfulContentHelper.Text(postCtaContentText);
        var preCtaButtonContent = ContentfulContentHelper.Text(preCtaContentText);
        var rightHandSideContent = ContentfulContentHelper.Text(sideContentText);

        mockContentParser.Setup(x => x.ToHtml(postCtaButtonContent)).ReturnsAsync(postCtaContentText);
        mockContentParser.Setup(x => x.ToHtml(preCtaButtonContent)).ReturnsAsync(preCtaContentText);
        mockContentParser.Setup(x => x.ToHtml(rightHandSideContent)).ReturnsAsync(sideContentText);

        var startPageResult = new StartPage
                              {
                                  CtaButtonText = "Start now",
                                  Header = "This is the header",
                                  PostCtaButtonContent = postCtaButtonContent,
                                  PreCtaButtonContent = preCtaButtonContent,
                                  RightHandSideContentHeader = "This is the side content header",
                                  RightHandSideContent = rightHandSideContent
                              };

        mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync(startPageResult);
        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as StartPageModel;
        model.Should().NotBeNull();
        model!.Header.Should().Be(startPageResult.Header);
        model.CtaButtonText.Should().Be(startPageResult.CtaButtonText);
        model.PostCtaButtonContent.Should().Be(postCtaContentText);
        model.PreCtaButtonContent.Should().Be(preCtaContentText);
        model.RightHandSideContent.Should().Be(sideContentText);
        model.RightHandSideContentHeader.Should().Be(startPageResult.RightHandSideContentHeader);

        mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);
    }
}
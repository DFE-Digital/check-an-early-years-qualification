using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
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
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockStartPageMapper = new Mock<IStartPageMapper>();

        var controller = new HomeController(mockLogger.Object, mockContentService.Object,
                                            mockUserJourneyCookieService.Object,
                                            mockStartPageMapper.Object);

        mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync((StartPage?)null);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyCritical("Start page content not found");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsContent_ReturnsStartPageModel()
    {
        var mockLogger = new Mock<ILogger<HomeController>>();
        var mockContentService = new Mock<IContentService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockStartPageMapper = new Mock<IStartPageMapper>();

        var controller = new HomeController(mockLogger.Object, mockContentService.Object,
                                            mockUserJourneyCookieService.Object,
                                            mockStartPageMapper.Object);

        var startPageResult = new StartPage
                              {
                                  Header = "This is the header",
                              };

        mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync(startPageResult);
        mockStartPageMapper.Setup(x => x.Map(startPageResult)).ReturnsAsync(new StartPageModel { Header = startPageResult.Header });
        
        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as StartPageModel;
        model.Should().NotBeNull();
        model.Header.Should().Be(startPageResult.Header);

        mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);
    }
}
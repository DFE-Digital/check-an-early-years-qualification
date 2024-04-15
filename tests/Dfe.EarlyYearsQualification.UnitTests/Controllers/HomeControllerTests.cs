using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HomeControllerTests
{
    private readonly ILogger<HomeController> _mockLogger = new NullLoggerFactory().CreateLogger<HomeController>();
    private Mock<IContentService> _mockContentService = new();
    private HomeController? _controller;

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new HomeController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNoContent_RedirectsToErrorPage()
    {
        _mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync((StartPage)default!);
        var result = await _controller!.Index();

        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("Error", resultType.ActionName);
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsContent_ReturnsStartPageModel()
    {
        var startPageResult = new StartPage
        {
            CtaButtonText = "Start now",
            Header = "This is the header",
            PostCtaButtonContentHtml = "This is the post cta content",
            PreCtaButtonContentHtml = "This is the pre cta content",
            RightHandSideContentHeader = "This is the side content header",
            RightHandSideContentHtml = "This is the side content"
        };

        _mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync(startPageResult);
        var result = await _controller!.Index();

        Assert.IsNotNull(result);
        var resultType = result as ViewResult;
        Assert.IsNotNull(resultType);
        var model = resultType.Model as StartPageModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(startPageResult.Header, model.Header);
        Assert.AreEqual(startPageResult.CtaButtonText, model.CtaButtonText);
        Assert.AreEqual(startPageResult.PostCtaButtonContentHtml, model.PostCtaButtonContent);
        Assert.AreEqual(startPageResult.PreCtaButtonContentHtml, model.PreCtaButtonContent);
        Assert.AreEqual(startPageResult.RightHandSideContentHtml, model.RightHandSideContent);
        Assert.AreEqual(startPageResult.RightHandSideContentHeader, model.RightHandSideContentHeader);
    }
}
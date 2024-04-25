using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HomeControllerTests
{
    private readonly ILogger<HomeController> _mockLogger = new NullLoggerFactory().CreateLogger<HomeController>();
    private HomeController? _controller;
    private Mock<IContentService> _mockContentService = new();

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

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Error");
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

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as StartPageModel;
        model.Should().NotBeNull();
        model!.Header.Should().Be(startPageResult.Header);
        model.CtaButtonText.Should().Be(startPageResult.CtaButtonText);
        model.PostCtaButtonContent.Should().Be(startPageResult.PostCtaButtonContentHtml);
        model.PreCtaButtonContent.Should().Be(startPageResult.PreCtaButtonContentHtml);
        model.RightHandSideContent.Should().Be(startPageResult.RightHandSideContentHtml);
        model.RightHandSideContentHeader.Should().Be(startPageResult.RightHandSideContentHeader);
    }
}
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
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
    [TestMethod]
    public async Task Index_ContentServiceReturnsNoContent_RedirectsToErrorPage()
    {
        var mockLogger = new NullLoggerFactory().CreateLogger<HomeController>();
        var mockHtmlRenderer = new Mock<IHtmlRenderer>();
        var mockSideRenderer = new Mock<ISideContentRenderer>();
        var mockContentService = new Mock<IContentService>();
        var controller = new HomeController(mockLogger, mockContentService.Object, mockHtmlRenderer.Object,
                                            mockSideRenderer.Object);

        mockContentService.Setup(x => x.GetStartPage()).ReturnsAsync((StartPage?)default);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsContent_ReturnsStartPageModel()
    {
        var mockLogger = new NullLoggerFactory().CreateLogger<HomeController>();
        var mockHtmlRenderer = new Mock<IHtmlRenderer>();
        var mockSideRenderer = new Mock<ISideContentRenderer>();
        var mockContentService = new Mock<IContentService>();
        var controller = new HomeController(mockLogger, mockContentService.Object, mockHtmlRenderer.Object,
                                            mockSideRenderer.Object);

        const string postCtaContentText = "This is the post cta content";
        const string preCtaContentText = "This is the pre cta content";
        const string sideContentText = "This is the side content";

        var postCtaButtonContent = ContentfulContentHelper.Text(postCtaContentText);
        var preCtaButtonContent = ContentfulContentHelper.Text(preCtaContentText);
        var rightHandSideContent = ContentfulContentHelper.Text(sideContentText);

        mockHtmlRenderer.Setup(x => x.ToHtml(postCtaButtonContent)).ReturnsAsync(postCtaContentText);
        mockHtmlRenderer.Setup(x => x.ToHtml(preCtaButtonContent)).ReturnsAsync(preCtaContentText);
        mockSideRenderer.Setup(x => x.ToHtml(rightHandSideContent)).ReturnsAsync(sideContentText);

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
    }
}
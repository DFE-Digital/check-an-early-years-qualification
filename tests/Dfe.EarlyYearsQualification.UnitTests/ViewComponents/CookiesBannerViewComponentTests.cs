using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.ViewComponents;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.ViewComponents;

[TestClass]
public class CookiesBannerViewComponentTests
{
    [TestMethod]
    public async Task InvokeAsync_NoContentFound_ThrowsAndReturnsShowFalse()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesBannerViewComponent>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();

        mockContentService.Setup(x => x.GetCookiesBannerContent()).ReturnsAsync((CookiesBanner?)default);

        var cookiesBannerViewComponent =
            new CookiesBannerViewComponent(mockContentService.Object, mockLogger.Object, mockHtmlRenderer.Object);

        var result = await cookiesBannerViewComponent.InvokeAsync();

        result.Should().BeAssignableTo<IViewComponentResult>();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = (CookiesBannerModel)model!;

        data.Show.Should().BeFalse();

        mockLogger.VerifyError("No content for the cookies banner");
    }

    [TestMethod]
    public async Task InvokeAsync_ContentFound_MapsContentAndReturnsModel()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesBannerViewComponent>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();

        var expectedContent = new CookiesBanner
                              {
                                  AcceptButtonText = "Test Accept Button Text",
                                  AcceptedCookiesContent = ContentfulContentHelper.Text("Some HTML"),
                                  CookiesBannerContent = ContentfulContentHelper.Text("Some HTML"),
                                  CookiesBannerLinkText = "Test Cookies Banner Link Text",
                                  CookiesBannerTitle = "Test Cookies Banner Title",
                                  HideCookieBannerButtonText = "Test Hide Cookies Banner Button Text",
                                  RejectButtonText = "Test Reject Button Text",
                                  RejectedCookiesContent = ContentfulContentHelper.Text("Some HTML")
                              };

        mockContentService.Setup(x => x.GetCookiesBannerContent()).ReturnsAsync(expectedContent);

        mockHtmlRenderer.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Mock HTML renderer result");

        var cookiesBannerViewComponent =
            new CookiesBannerViewComponent(mockContentService.Object, mockLogger.Object, mockHtmlRenderer.Object);

        var result = await cookiesBannerViewComponent.InvokeAsync();

        result.Should().BeAssignableTo<IViewComponentResult>();

        var model = (result as ViewViewComponentResult)?.ViewData?.Model;
        model.Should().NotBeNull();

        var data = (CookiesBannerModel)model!;

        data.AcceptButtonText.Should().Be(expectedContent.AcceptButtonText);
        data.AcceptedCookiesContent.Should().Be("Mock HTML renderer result");
        data.CookiesBannerContent.Should().Be("Mock HTML renderer result");
        data.CookiesBannerLinkText.Should().Be(expectedContent.CookiesBannerLinkText);
        data.CookiesBannerTitle.Should().Be(expectedContent.CookiesBannerTitle);
        data.RejectButtonText.Should().Be(expectedContent.RejectButtonText);
        data.RejectedCookiesContent.Should().Be("Mock HTML renderer result");
        data.HideCookieBannerButtonText.Should().Be("Test Hide Cookies Banner Button Text");
        data.Show.Should().BeTrue();

        mockHtmlRenderer.Verify(x => x.ToHtml(expectedContent.CookiesBannerContent), Times.Once);
    }
}
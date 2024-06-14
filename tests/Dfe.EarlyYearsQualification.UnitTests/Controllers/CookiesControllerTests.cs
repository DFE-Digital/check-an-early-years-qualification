using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.CookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CookiesControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync((CookiesPage?)default);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the cookies page");
    }

    [TestMethod]
    public async Task Index_ContentFound_ReturnsCorrectModel()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var expectedContent = new CookiesPage
                              {
                                  Heading = "Test Heading",
                                  ButtonText = "Test Button Text",
                                  Options =
                                  [
                                      new Option
                                      {
                                          Label = "Click Me!",
                                          Value = "test option 1"
                                      },

                                      new Option
                                      {
                                          Label = "No Click Me!",
                                          Value = "test option 2"
                                      }
                                  ],
                                  SuccessBannerHeading = "Test success banner heading",
                                  ErrorText = "Test error text"
                              };

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync(expectedContent);

        mockHtmlTableRenderer.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test main content");
        mockSuccessBannerRenderer.Setup(x => x.ToHtml(It.IsAny<Document>()))
                                 .ReturnsAsync("Test success banner content");

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<ViewResult>()
              .Which.Model
              .Should()
              .BeEquivalentTo(new CookiesPageModel
                              {
                                  Heading = expectedContent.Heading,
                                  BodyContent = "Test main content",
                                  ButtonText = expectedContent.ButtonText,
                                  Options =
                                  [
                                      new OptionModel
                                      {
                                          Label = expectedContent.Options[0].Label,
                                          Value = expectedContent.Options[0].Value
                                      },

                                      new OptionModel
                                      {
                                          Label = expectedContent.Options[1].Label,
                                          Value = expectedContent.Options[1].Value
                                      }
                                  ],
                                  SuccessBannerHeading = expectedContent.SuccessBannerHeading,
                                  SuccessBannerContent = "Test success banner content",
                                  ErrorText = expectedContent.ErrorText
                              });
    }

    [TestMethod]
    public void Accept_EndpointCalled_CallsToSetPreferenceAndChecksUrl()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.Accept("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("some URL");

        mockCookieService.Verify(x => x.SetPreference(true), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void Accept_EndpointCalledWithBadUrl_CallsToSetPreferenceAndRedirectsToCookiesPage()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.Accept("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("/cookies");

        mockCookieService.Verify(x => x.SetPreference(true), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void Reject_EndpointCalled_CallsToRejectAndChecksUrl()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.Reject("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("some URL");

        mockCookieService.Verify(x => x.RejectCookies(), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void Reject_EndpointCalledWithBadUrl_CallsToRejectAndRedirectsToCookiePage()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.Reject("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("/cookies");

        mockCookieService.Verify(x => x.RejectCookies(), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void HideBanner_EndpointCalled_CallsToSetVisibilityAndChecksUrl()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.HideBanner("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("some URL");

        mockCookieService.Verify(x => x.SetVisibility(false), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void HideBanner_EndpointCalledWithBadUrl_CallsToRejectAndRedirectsToCookiePage()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object);

        var result = controller.HideBanner("some URL");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("/cookies");

        mockCookieService.Verify(x => x.SetVisibility(false), Times.Once);
        mockUrlChecker.Verify(x => x.IsLocalUrl("some URL"), Times.Once);
    }

    [TestMethod]
    public void CookiePreference_Accept_CallsToSetPreferenceAndRedirects()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object)
                         {
                             TempData = tempData
                         };

        var result = controller.CookiePreference("all-cookies");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("/cookies");

        controller.TempData["UserPreferenceRecorded"].Should().Be(true);

        mockCookieService.Verify(x => x.SetPreference(true), Times.Once);
    }

    [TestMethod]
    public void CookiePreference_Reject_CallsToSetPreferenceAndRedirects()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();
        var mockCookieService = new Mock<ICookieService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object,
                                               mockCookieService.Object, mockUrlChecker.Object)
                         {
                             TempData = tempData
                         };

        var result = controller.CookiePreference("anything-but-all-cookies");

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<RedirectResult>()
              .Which.Url.Should().Be("/cookies");

        controller.TempData["UserPreferenceRecorded"].Should().Be(true);

        mockCookieService.Verify(x => x.RejectCookies(), Times.Once);
    }
}
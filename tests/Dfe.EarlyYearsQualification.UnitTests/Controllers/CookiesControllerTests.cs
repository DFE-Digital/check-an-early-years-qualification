using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CookiesPreferenceControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
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
                                  ErrorText = "Test error text",
                                  Body = ContentfulContentHelper.Paragraph("Test Body"),
                                  FormHeading = "Test form heading",
                                  SuccessBannerContent = ContentfulContentHelper.Paragraph("Test success banner"),
                                  BackButton = new NavigationLink()
                                               {
                                                   DisplayText = "Test display text",
                                                   OpenInNewTab = false,
                                                   Href = "Test Href"
                                               }
                              };

        mockHtmlRenderer.Setup(x => x.ToHtml(expectedContent.Body)).ReturnsAsync("Formatted Body");
        mockHtmlRenderer.Setup(x => x.ToHtml(expectedContent.SuccessBannerContent)).ReturnsAsync("Formatted Success Banner Content");

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync(expectedContent);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object,
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
                                  BodyContent = "Formatted Body",
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
                                  SuccessBannerContent = "Formatted Success Banner Content",
                                  ErrorText = expectedContent.ErrorText,
                                  FormHeading = "Test form heading",
                                  BackButton = new NavigationLinkModel
                                               {
                                                   DisplayText = "Test display text",
                                                   OpenInNewTab = false,
                                                   Href = "Test Href"
                                               }
                              });
    }

    [TestMethod]
    public void Accept_EndpointCalled_CallsToSetPreferenceAndChecksUrl()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object,
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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object);

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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object)
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
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockHtmlRenderer = new Mock<IGovUkContentfulParser>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockHtmlRenderer.Object, mockCookieService.Object, mockUrlChecker.Object)
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
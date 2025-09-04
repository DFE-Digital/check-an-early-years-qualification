using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class CookiesPreferenceControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync((CookiesPage?)null);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        var expectedModel = new CookiesPageModel { Heading = "test" };
        mockCookiePageMapper.Setup(x => x.Map(It.IsAny<CookiesPage>())).ReturnsAsync(expectedModel).Verifiable();
        
        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync(new CookiesPage()).Verifiable();

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<ViewResult>()
              .Which.Model
              .Should()
              .BeEquivalentTo(expectedModel);
        
        mockContentService.VerifyAll();
        mockCookieService.VerifyAll();
    }

    [TestMethod]
    public void Accept_EndpointCalled_CallsToSetPreferenceAndChecksUrl()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesPreferenceController>>();
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(true);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        mockUrlChecker.Setup(x => x.IsLocalUrl("some URL")).Returns(false);

        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object);

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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object)
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
        var mockCookieService = new Mock<ICookiesPreferenceService>();
        var mockUrlChecker = new Mock<IUrlHelper>();
        var mockCookiePageMapper = new Mock<ICookiesPageMapper>();

        var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object)
                       {
                           ["UserPreferenceRecorded"] = false
                       };
        var controller = new CookiesPreferenceController(mockLogger.Object, mockContentService.Object,
                                                         mockCookieService.Object,
                                                         mockUrlChecker.Object,
                                                         mockCookiePageMapper.Object)
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
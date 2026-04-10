using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Dfe.EarlyYearsQualification.Web.Services.WebView;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationListControllerTests
{
    [TestMethod]
    public async Task Index_WebViewServiceReturnsNull_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object);

        mockWebViewService.Setup(x => x.GetWebViewPage()).ReturnsAsync((WebViewPage?)null);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Web view page content could not be found");
    }

    [TestMethod]
    public async Task Index_WebViewServiceReturnsContent_ReturnsViewModel()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object);

        var webViewPage = new WebViewPage();
        var expectedModel = new EarlyYearsQualificationListModel();

        mockWebViewService.Setup(x => x.GetWebViewPage()).ReturnsAsync(webViewPage);
        mockWebViewService.Setup(x => x.MapWebViewPageContentToViewModelAsync(webViewPage)).ReturnsAsync(expectedModel);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as EarlyYearsQualificationListModel;
        model.Should().NotBeNull();
        model.Should().BeSameAs(expectedModel);

        mockWebViewService.Verify(x => x.GetWebViewPage(), Times.Once);
        mockWebViewService.Verify(x => x.MapWebViewPageContentToViewModelAsync(webViewPage), Times.Once);
    }

    [TestMethod]
    public void ClearFilters_CallsWebViewServiceSetWebViewFilters_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object);

        var result = controller.ClearFilters();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.SetWebViewFilters(It.IsAny<WebViewFilters>()), Times.Once);
    }

    [TestMethod]
    public void ApplyFilter_CallsWebViewServiceApplyFilters_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object);

        var model = new EarlyYearsQualificationListModel();

        var result = controller.ApplyFilter(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.ApplyFilters(model), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_CallsWebViewServiceRemoveFilter_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object);

        var filter = "test-filter";

        var result = controller.RemoveFilter(filter);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.RemoveFilter(filter), Times.Once);
    }
}

using System.Text;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
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
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);

        mockWebViewService.Setup(x => x.GetWebViewPage()).ReturnsAsync((WebViewPage?)null);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Web view page content could not be found");
    }

    [TestMethod]
    public async Task Index_WebViewServiceReturnsContent_ReturnsViewModel()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);

        var webViewPage = new WebViewPage();
        var expectedModel = new EarlyYearsQualificationListModel();

        const bool isProductionEnvironment = true;

        mockWebViewService.Setup(x => x.GetWebViewPage()).ReturnsAsync(webViewPage);
        mockEnvironmentService.Setup(x => x.IsProduction()).Returns(isProductionEnvironment);
        mockWebViewService.Setup(x => x.MapWebViewPageContentToViewModelAsync(webViewPage, isProductionEnvironment)).ReturnsAsync(expectedModel);

        var result = await controller.Index();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as EarlyYearsQualificationListModel;
        model.Should().NotBeNull();
        model.Should().BeSameAs(expectedModel);

        mockWebViewService.Verify(x => x.GetWebViewPage(), Times.Once);
        mockWebViewService.Verify(x => x.MapWebViewPageContentToViewModelAsync(webViewPage, isProductionEnvironment), Times.Once);
    }

    [TestMethod]
    public void ClearFilters_CallsWebViewServiceSetWebViewFilters_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);

        var result = controller.ClearFilters();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.SetWebViewFilters(It.IsAny<WebViewFilters>()), Times.Once);
    }

    [TestMethod]
    public void ApplyFilter_CallsWebViewServiceApplyFilters_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);

        var model = new EarlyYearsQualificationListModel();

        var result = controller.ApplyFilter(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.ApplyFilters(model), Times.Once);
    }

    [TestMethod]
    public void RemoveFilter_CallsWebViewServiceRemoveFilter_RedirectsToIndex()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);

        const string filter = "test-filter";

        var result = controller.RemoveFilter(filter);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");

        mockWebViewService.Verify(x => x.RemoveFilter(filter), Times.Once);
    }
    
    [TestMethod]
    public async Task Download_CallsQualificationDownloadService_ReturnsFileContent()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);
        
        (byte[] fileContents, string fileName) = (Encoding.UTF8.GetBytes("test"), "test.csv");

        mockQualificationDownloadService.Setup(x => x.GetEyqlDownload(It.IsAny<string>()))
                                        .ReturnsAsync((fileContents, fileName));

        var result = await controller.Download();

        result.Should().NotBeNull();

        var resultType = result as FileContentResult;
        resultType.Should().NotBeNull();
        resultType.FileContents.Should().NotBeEmpty();
        resultType.FileContents.Should().Equal(fileContents);
        resultType.FileDownloadName.Should().Be(fileName);

        mockQualificationDownloadService.Verify(x => x.GetEyqlDownload("Development"), Times.Once);
    }
    
    [TestMethod]
    public async Task Download_QualificationDownloadServiceReturnsEmptyArray_RedirectsToError()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);


        (byte[] fileContents, string fileName) = (Array.Empty<byte>(), "test.csv");

        mockQualificationDownloadService.Setup(x => x.GetEyqlDownload(It.IsAny<string>()))
                                        .ReturnsAsync((fileContents, fileName));

        var result = await controller.Download();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Null or empty EYQL content returned");
        mockQualificationDownloadService.Verify(x => x.GetEyqlDownload("Development"), Times.Once);
    }

    [TestMethod]
    public async Task InternalDownload_EnvironmentIsProduction_ReturnsNotFound()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();
        
        mockEnvironmentService.Setup(x => x.IsProduction()).Returns(true);

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);
        
        var result = await controller.InternalDownload();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [TestMethod]
    public async Task InternalDownload_EyqlContentIsNull_ReturnsRedirectToError()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();
        
        mockEnvironmentService.Setup(x => x.IsProduction()).Returns(false);
        mockQualificationDownloadService.Setup(x => x.GetEyqlDataForInternalDownload())
                                        .ReturnsAsync(Array.Empty<byte>());

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);
        
        
        var result = await controller.InternalDownload();
        result.Should().NotBeNull();
        result.Should().BeOfType<RedirectToActionResult>();
        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }
    
    [TestMethod]
    public async Task InternalDownload_EyqlContentIsNotNull_ReturnsFileResult()
    {
        var mockLogger = new Mock<ILogger<QualificationListController>>();
        var mockWebViewService = new Mock<IWebViewService>();
        var mockQualificationDownloadService = new Mock<IQualificationDownloadService>();
        var mockEnvironmentService = CreateEnvironmentService();
        
        var mockContent = Encoding.UTF8.GetBytes("this is a test");
        mockEnvironmentService.Setup(x => x.IsProduction()).Returns(false);
        mockQualificationDownloadService.Setup(x => x.GetEyqlDataForInternalDownload())
                                        .ReturnsAsync(mockContent);
        
        var expectedFileName = $"published_qualifications_{DateTime.Now.ToShortDateString()}.csv";

        var controller = new QualificationListController(mockLogger.Object, mockWebViewService.Object,
                                                         mockQualificationDownloadService.Object,
                                                         mockEnvironmentService.Object);
        
        
        
        var result = await controller.InternalDownload();
        result.Should().NotBeNull();
        result.Should().BeOfType<FileContentResult>();
        var resultType = result as FileContentResult;
        resultType.Should().NotBeNull();
        resultType.FileContents.Should().NotBeEmpty();
        resultType.FileContents.Should().Equal(mockContent);
        resultType.FileDownloadName.Should().Be(expectedFileName);
    }

    private static Mock<IEnvironmentService> CreateEnvironmentService(string environment = "Development")
    {
        var mockEnvironmentService = new Mock<IEnvironmentService>();
        mockEnvironmentService.Setup(x => x.GetEnvironment()).Returns(environment);
        return mockEnvironmentService;
    }
}

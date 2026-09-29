using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationSearchControllerTests
{

    [TestMethod]
    public async Task Get_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                 mockQualificationSearchService.Object)
               {
                   ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       }
               };
        
        mockQualificationSearchService.Setup(o => o.GetQualifications()).ReturnsAsync(new QualificationListModel());

        var result = await controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task Get_NoContent_LogsAndRedirectsToError()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };

        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public async Task Get_Calls_Service_GetQualifications()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };
        await controller.Get();

        mockQualificationSearchService.Verify(x => x.GetQualifications(), Times.Once);
    }

    [TestMethod]
    public async Task Get_NullQualifications_LogsAndRedirectsToError()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };
        mockQualificationSearchService.Setup(o => o.GetQualifications()).ReturnsAsync((QualificationListModel)null!);
        
        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public void Refine_WithSearch_CallsService_WithSearch()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };
        
        const string search = "Test";

        controller.Refine(search);

        mockQualificationSearchService.Verify(x => x.Refine(search), Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_CallsService_WithEmptyString()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };

        controller.Refine(null);

        mockQualificationSearchService.Verify(x => x.Refine(string.Empty), Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };
        
        var result = controller.Refine(null);

        result.Should().BeOfType<RedirectToActionResult>();
        var actionResult = (RedirectToActionResult)result;
        actionResult.ActionName.Should().Be("Get");
    }

    [TestMethod]
    public void Refine_WithSearch_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };
        
        var result = controller.Refine("Test");

        result.Should().BeOfType<RedirectToActionResult>();
        var actionResult = (RedirectToActionResult)result;
        actionResult.ActionName.Should().Be("Get");
    }

    [TestMethod]
    public void Refine_InvalidModel_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockQualificationSearchService = new Mock<IQualificationSearchService>();
        var controller = new QualificationSearchController(mockLogger.Object,
                                                           mockQualificationSearchService.Object)
                         {
                             ControllerContext = new ControllerContext
                                                 {
                                                     HttpContext = new DefaultHttpContext()
                                                 }
                         };

        controller.ModelState.AddModelError("Key", "Error message");

        controller.Refine(null);

        mockLogger
            .VerifyWarning($"Invalid model state in {nameof(QualificationSearchController)} POST");
    }
}
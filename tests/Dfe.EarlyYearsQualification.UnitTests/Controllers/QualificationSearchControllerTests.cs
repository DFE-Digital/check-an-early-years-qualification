using Dfe.EarlyYearsQualification.TestSupport;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Microsoft.AspNetCore.Http;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationSearchControllerTests
{
    private Mock<ILogger<QualificationSearchController>> _mockLogger =
        new Mock<ILogger<QualificationSearchController>>();

    private Mock<IQualificationSearchService> _mockQualificationSearchService = new Mock<IQualificationSearchService>();

    private QualificationSearchController GetSut()
    {
        return new QualificationSearchController(_mockLogger.Object,
                                                 _mockQualificationSearchService.Object)
               {
                   ControllerContext = new ControllerContext
                                       {
                                           HttpContext = new DefaultHttpContext()
                                       }
               };
    }

    [TestInitialize]
    public void Initialize()
    {
        _mockLogger = new Mock<ILogger<QualificationSearchController>>();
        _mockQualificationSearchService = new Mock<IQualificationSearchService>();
    }

    [TestMethod]
    public async Task Get_ReturnsView()
    {
        _mockQualificationSearchService.Setup(o => o.GetQualifications()).ReturnsAsync(new QualificationListModel());
        var controller = GetSut();

        var result = await controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task Get_NoContent_LogsAndRedirectsToError()
    {
        var controller = GetSut();

        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public async Task Get_Calls_Service_GetQualifications()
    {
        var controller = GetSut();
        await controller.Get();

        _mockQualificationSearchService.Verify(x => x.GetQualifications(), Times.Once);
    }

    [TestMethod]
    public async Task Get_NullQualifications_LogsAndRedirectsToError()
    {
        _mockQualificationSearchService.Setup(o => o.GetQualifications()).ReturnsAsync((QualificationListModel)null!);

        var controller = GetSut();
        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public void Refine_WithSearch_CallsService_WithSearch()
    {
        const string search = "Test";
        var controller = GetSut();

        controller.Refine(search);

        _mockQualificationSearchService.Verify(x => x.Refine(search), Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_CallsService_WithEmptyString()
    {
        var controller = GetSut();

        controller.Refine(null);

        _mockQualificationSearchService.Verify(x => x.Refine(string.Empty), Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_RedirectsToGet()
    {
        var controller = GetSut();

        var result = controller.Refine(null);

        result.Should().BeOfType<RedirectToActionResult>();
        var actionResult = (RedirectToActionResult)result;
        actionResult.ActionName.Should().Be("Get");
    }

    [TestMethod]
    public void Refine_WithSearch_RedirectsToGet()
    {
        var controller = GetSut();

        var result = controller.Refine("Test");

        result.Should().BeOfType<RedirectToActionResult>();
        var actionResult = (RedirectToActionResult)result;
        actionResult.ActionName.Should().Be("Get");
    }

    [TestMethod]
    public void Refine_InvalidModel_LogsWarning()
    {
        var controller = GetSut();

        controller.ModelState.AddModelError("Key", "Error message");

        controller.Refine(null);

        _mockLogger
            .VerifyWarning($"Invalid model state in {nameof(QualificationSearchController)} POST");
    }
}
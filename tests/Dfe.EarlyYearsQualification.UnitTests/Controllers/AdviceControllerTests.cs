using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AdviceControllerTests
{

    private readonly ILogger<AdviceController> _mockLogger = new NullLoggerFactory().CreateLogger<AdviceController>();
    private Mock<IContentService> _mockContentService = new();
    private AdviceController? _controller;

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new AdviceController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        _mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk)).ReturnsAsync((AdvicePage)default!).Verifiable();
        var result = await _controller!.QualificationOutsideTheUnitedKingdom();

        _mockContentService.VerifyAll();
        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("Error", resultType.ActionName);
        Assert.AreEqual("Home", resultType.ControllerName);
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var advicePage = new AdvicePage { Heading = "Heading", BodyHtml = "Test html body" };
        _mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk)).ReturnsAsync(advicePage);
        var result = await _controller!.QualificationOutsideTheUnitedKingdom();

        Assert.IsNotNull(result);
        var resultType = result as ViewResult;
        Assert.IsNotNull(resultType);
        var model = resultType.Model as AdvicePageModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(advicePage.Heading, model.Heading);
        Assert.AreEqual(advicePage.BodyHtml, model.BodyContent);
    }
}
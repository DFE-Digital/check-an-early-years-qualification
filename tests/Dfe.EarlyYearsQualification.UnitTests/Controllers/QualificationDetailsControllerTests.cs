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
public class QualificationDetailsControllerTests
{

    private ILogger<QualificationDetailsController> _mockLogger = new NullLoggerFactory().CreateLogger<QualificationDetailsController>();
    private Mock<IContentService> _mockContentService = new Mock<IContentService>();
    private QualificationDetailsController? _controller;

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new QualificationDetailsController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task Index_PassInNullQualificationId_ReturnsBadRequest()
    {
        var result = await _controller!.Index(string.Empty);

        Assert.IsNotNull(result);
        var resultType = result as BadRequestResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual(400, resultType.StatusCode);
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNoQualification_RedirectsToErrorPage()
    {
        var qualificationId = "eyq-145";
        _mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync((Qualification)default!);
        var result = await _controller!.Index(qualificationId);

        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("Error", resultType.ActionName);
        Assert.AreEqual("Home", resultType.ControllerName);
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsQualification_ReturnsQualificationDetailsModel()
    {
        var qualificationId = "eyq-145";
        var qualificationResult = new Qualification(qualificationId, "Qualification Name", "NCFE", "2", "2014", "2019", "ABC/547/900", "notes", "additonal requirements");
        _mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync(qualificationResult);
        var result = await _controller!.Index(qualificationId);

        Assert.IsNotNull(result);
        var resultType = result as ViewResult;
        Assert.IsNotNull(resultType);
        var model = resultType.Model as QualificationDetailsModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(qualificationResult.QualificationId, model.QualificationId);
        Assert.AreEqual(qualificationResult.QualificationName, model.QualificationName);
        Assert.AreEqual(qualificationResult.AwardingOrganisationTitle, model.AwardingOrganisationTitle);
        Assert.AreEqual(qualificationResult.QualificationLevel, model.QualificationLevel);
        Assert.AreEqual(qualificationResult.FromWhichYear, model.FromWhichYear);
        Assert.AreEqual(qualificationResult.ToWhichYear, model.ToWhichYear);
        Assert.AreEqual(qualificationResult.QualificationNumber, model.QualificationNumber);
        Assert.AreEqual(qualificationResult.Notes, model.Notes);
        Assert.AreEqual(qualificationResult.AdditionalRequirements, model.AdditionalRequirements);
    }
}
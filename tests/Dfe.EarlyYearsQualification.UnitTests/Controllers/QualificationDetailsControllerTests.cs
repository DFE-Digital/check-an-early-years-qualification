using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationDetailsControllerTests
{
    private readonly ILogger<QualificationDetailsController> _mockLogger =
        new NullLoggerFactory().CreateLogger<QualificationDetailsController>();

    private QualificationDetailsController? _controller;
    private Mock<IContentService> _mockContentService = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new QualificationDetailsController(_mockLogger, _mockContentService.Object)
                      {
                          ControllerContext = new ControllerContext
                                              {
                                                  HttpContext = new DefaultHttpContext()
                                              }
                      };
    }

    [TestMethod]
    public async Task Index_PassInNullQualificationId_ReturnsBadRequest()
    {
        var result = await _controller!.Index(string.Empty);

        result.Should().NotBeNull();

        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType!.StatusCode.Should().Be(400);
    }


    [TestMethod]
    public async Task Index_ContentServiceReturnsNullDetailsPage_RedirectsToHomeError()
    {
        _mockContentService.Setup(s => s.GetDetailsPage())
                           .ReturnsAsync((DetailsPage?)null);

        var result = await _controller!.Index("X");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Error");
        actionResult.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNoQualification_RedirectsToErrorPage()
    {
        const string qualificationId = "eyq-145";
        _mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync((Qualification?)default);
        _mockContentService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        var result = await _controller!.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsQualification_ReturnsQualificationDetailsModel()
    {
        const string qualificationId = "eyq-145";
        var qualificationResult = new Qualification(qualificationId, "Qualification Name", "NCFE", 2, "2014", "2019",
                                                    "ABC/547/900", "notes", "additonal requirements");
        _mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync(qualificationResult);
        _mockContentService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        var result = await _controller!.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QualificationDetailsModel;
        model.Should().NotBeNull();

        model!.QualificationId.Should().Be(qualificationResult.QualificationId);
        model.QualificationName.Should().Be(qualificationResult.QualificationName);
        model.AwardingOrganisationTitle.Should().Be(qualificationResult.AwardingOrganisationTitle);
        model.QualificationLevel.Should().Be(qualificationResult.QualificationLevel);
        model.FromWhichYear.Should().Be(qualificationResult.FromWhichYear);
        model.ToWhichYear.Should().Be(qualificationResult.ToWhichYear);
        model.QualificationNumber.Should().Be(qualificationResult.QualificationNumber);
        model.Notes.Should().Be(qualificationResult.Notes);
        model.AdditionalRequirements.Should().Be(qualificationResult.AdditionalRequirements);
    }

    [TestMethod]
    public void Get_ReturnsView()
    {
        var result = _controller!.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
}
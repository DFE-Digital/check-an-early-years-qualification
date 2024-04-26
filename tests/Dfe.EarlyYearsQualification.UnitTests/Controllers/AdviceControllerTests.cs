using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AdviceControllerTests
{
    private readonly ILogger<AdviceController> _mockLogger = new NullLoggerFactory().CreateLogger<AdviceController>();
    private AdviceController? _controller;
    private Mock<IContentService> _mockContentService = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new AdviceController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        _mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                           .ReturnsAsync((AdvicePage)default!).Verifiable();
        var result = await _controller!.QualificationOutsideTheUnitedKingdom();

        _mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var advicePage = new AdvicePage { Heading = "Heading", BodyHtml = "Test html body" };
        _mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                           .ReturnsAsync(advicePage);
        var result = await _controller!.QualificationOutsideTheUnitedKingdom();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(advicePage.BodyHtml);
    }
}
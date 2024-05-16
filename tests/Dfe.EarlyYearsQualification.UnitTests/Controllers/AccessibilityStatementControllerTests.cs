using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AccessibilityStatementControllerTests
{
  private Mock<IContentService> _mockContentService = new();
  private AccessibilityStatementController? _controller;

  private Mock<ILogger<AccessibilityStatementController>>? _mockLogger;

  [TestInitialize]
  public void BeforeEachTest()
  {
    _mockContentService = new Mock<IContentService>();
    _mockLogger = new Mock<ILogger<AccessibilityStatementController>>();
    
    _controller = new AccessibilityStatementController(_mockLogger.Object, _mockContentService.Object);
  }

  [TestMethod]
  public async Task Index_NoContent_NavigatesToErrorPageAsync()
  {
    _mockContentService.Setup(x => x.GetAccessibilityStatementPage()).ReturnsAsync((AccessibilityStatementPage)default!);
    _controller = new AccessibilityStatementController(_mockLogger!.Object, _mockContentService.Object);

    var result = await _controller!.Index();

    result.Should().NotBeNull();
    result.Should().BeEquivalentTo(new RedirectToActionResult("Error", "Home", null));

    _mockLogger.VerifyError("No content for the accessibility statement page");
  }

  [TestMethod]
  public async Task Index_ContentFound_ReturnsCorrectModel()
  {
    var expectedContent = new AccessibilityStatementPage()
    {
      Heading = "Test Heading",
      BodyHtml = "<p> Test Body </p>",
    };

    _mockContentService.Setup(x => x.GetAccessibilityStatementPage()).ReturnsAsync(expectedContent);

    _controller = new AccessibilityStatementController(_mockLogger!.Object, _mockContentService.Object);

    var result = await _controller!.Index();

    result.Should().NotBeNull();
    result.Should()
          .BeAssignableTo<ViewResult>()
          .Which.Model
                .Should()
                .BeEquivalentTo(new AccessibilityStatementPageModel()
                {
                  Heading = expectedContent.Heading,
                  BodyContent = expectedContent.BodyHtml,
                });
  }
}
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
public class AccessibilityStatementControllerTests
{
  private Mock<IContentService> _mockContentService = new();
  private AccessibilityStatementController? _controller;

  private readonly ILogger<AccessibilityStatementController> _mockLogger = new NullLoggerFactory().CreateLogger<AccessibilityStatementController>();

  [TestInitialize]
  public void BeforeEachTest()
  {
    _mockContentService = new Mock<IContentService>();
    _controller = new AccessibilityStatementController(_mockLogger, _mockContentService.Object);
  }

  [TestMethod]
  public async Task Index_NoContent_NavigatesToErrorPageAsync()
  {
    _mockContentService.Setup(x => x.GetAccessibilityStatementPage()).ReturnsAsync((AccessibilityStatementPage)default!);
    _controller = new AccessibilityStatementController(_mockLogger, _mockContentService.Object);

    var result = await _controller!.Index();

    Assert.IsNotNull(result);
    var resultType = result as RedirectToActionResult;
    Assert.IsNotNull(resultType);
    Assert.AreEqual("Error", resultType.ActionName);
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

    _controller = new AccessibilityStatementController(_mockLogger, _mockContentService.Object);

    var result = await _controller!.Index();
    Assert.IsNotNull(result);
    var resultType = result as ViewResult;
    Assert.IsNotNull(resultType);
    var model = resultType.Model as AccessibilityStatementPageModel;
    Assert.IsNotNull(model);
    Assert.AreEqual(expectedContent.Heading, model.Heading);
    Assert.AreEqual(expectedContent.BodyHtml, model.BodyContent);
  }
}
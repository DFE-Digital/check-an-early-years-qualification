using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AccessibilityStatementControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<AccessibilityStatementController>>();
        var mockAccessibilityStatementMapper = new Mock<IAccessibilityStatementMapper>();

        var controller =
            new AccessibilityStatementController(mockLogger.Object, mockContentService.Object,
                                                 mockAccessibilityStatementMapper.Object);

        mockContentService.Setup(x => x.GetAccessibilityStatementPage())
                          .ReturnsAsync((AccessibilityStatementPage?)null);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Index", "Error", null));

        mockLogger.VerifyError("No content for the accessibility statement page");
    }

    [TestMethod]
    public async Task Index_ContentFound_ReturnsCorrectModel()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<AccessibilityStatementController>>();
        var mockAccessibilityStatementMapper = new Mock<IAccessibilityStatementMapper>();

        var controller =
            new AccessibilityStatementController(mockLogger.Object, mockContentService.Object,
                                                 mockAccessibilityStatementMapper.Object);

        const string heading = "Heading";
        const string body = "Body";
        var expectedContent = new AccessibilityStatementPage
                              {
                                  Heading = heading,
                                  Body = ContentfulContentHelper.Paragraph(body)
                              };

        mockContentService.Setup(x => x.GetAccessibilityStatementPage()).ReturnsAsync(expectedContent);

        mockAccessibilityStatementMapper.Setup(x => x.Map(It.IsAny<AccessibilityStatementPage>()))
                                        .ReturnsAsync(new AccessibilityStatementPageModel { BodyContent = body, Heading = heading});

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<ViewResult>()
              .Which.Model
              .Should()
              .BeEquivalentTo(new AccessibilityStatementPageModel
                              {
                                  Heading = expectedContent.Heading,
                                  BodyContent = body
                              });
    }
}
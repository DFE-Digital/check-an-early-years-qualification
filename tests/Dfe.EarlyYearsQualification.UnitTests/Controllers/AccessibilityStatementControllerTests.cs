using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AccessibilityStatementControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<AccessibilityStatementController>>();

        var mockRenderer = new Mock<IGovUkContentfulParser>();

        var controller =
            new AccessibilityStatementController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        mockContentService.Setup(x => x.GetAccessibilityStatementPage())
                          .ReturnsAsync((AccessibilityStatementPage?)default);


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

        var mockRenderer = new Mock<IGovUkContentfulParser>();

        var expectedContent = new AccessibilityStatementPage
                              {
                                  Heading = "Test Heading"
                              };

        mockContentService.Setup(x => x.GetAccessibilityStatementPage()).ReturnsAsync(expectedContent);

        const string expectedHtml = "<p>Some HTML.</p>";

        mockRenderer.Setup(x => x.ToHtml(It.IsAny<Document>()))
                    .ReturnsAsync(expectedHtml);

        var controller =
            new AccessibilityStatementController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<ViewResult>()
              .Which.Model
              .Should()
              .BeEquivalentTo(new AccessibilityStatementPageModel
                              {
                                  Heading = expectedContent.Heading,
                                  BodyContent = expectedHtml
                              });
    }
}
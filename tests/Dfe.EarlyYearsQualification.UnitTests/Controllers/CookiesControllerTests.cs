using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
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
public class CookiesControllerTests
{
    [TestMethod]
    public async Task Index_NoContent_NavigatesToErrorPageAsync()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object);

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync((CookiesPage?)default);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new RedirectToActionResult("Error", "Home", null));

        mockLogger.VerifyError("No content for the cookies page");
    }

    [TestMethod]
    public async Task Index_ContentFound_ReturnsCorrectModel()
    {
        var mockContentService = new Mock<IContentService>();
        var mockLogger = new Mock<ILogger<CookiesController>>();
        var mockHtmlTableRenderer = new Mock<IHtmlTableRenderer>();
        var mockSuccessBannerRenderer = new Mock<ISuccessBannerRenderer>();

        var expectedContent = new CookiesPage
                              {
                                  Heading = "Test Heading",
                                  ButtonText = "Test Button Text",
                                  Options =
                                  [
                                      new Option
                                      {
                                          Label = "Click Me!",
                                          Value = "test option 1"
                                      },

                                      new Option
                                      {
                                          Label = "No Click Me!",
                                          Value = "test option 2"
                                      }
                                  ],
                                  SuccessBannerHeading = "Test success banner heading",
                                  ErrorText = "Test error text"
                              };

        mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync(expectedContent);

        mockHtmlTableRenderer.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test main content");
        mockSuccessBannerRenderer.Setup(x => x.ToHtml(It.IsAny<Document>()))
                                 .ReturnsAsync("Test success banner content");

        var controller = new CookiesController(mockLogger.Object, mockContentService.Object,
                                               mockHtmlTableRenderer.Object, mockSuccessBannerRenderer.Object);

        var result = await controller.Index();

        result.Should().NotBeNull();
        result.Should()
              .BeAssignableTo<ViewResult>()
              .Which.Model
              .Should()
              .BeEquivalentTo(new CookiesPageModel
                              {
                                  Heading = expectedContent.Heading,
                                  BodyContent = "Test main content",
                                  ButtonText = expectedContent.ButtonText,
                                  Options =
                                  [
                                      new OptionModel
                                      {
                                          Label = expectedContent.Options[0].Label,
                                          Value = expectedContent.Options[0].Value
                                      },

                                      new OptionModel
                                      {
                                          Label = expectedContent.Options[1].Label,
                                          Value = expectedContent.Options[1].Value
                                      }
                                  ],
                                  SuccessBannerHeading = expectedContent.SuccessBannerHeading,
                                  SuccessBannerContent = "Test success banner content",
                                  ErrorText = expectedContent.ErrorText
                              });
    }
}
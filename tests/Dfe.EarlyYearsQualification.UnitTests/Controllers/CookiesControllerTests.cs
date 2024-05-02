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
public class CookiesControllerTests
{
  private Mock<IContentService> _mockContentService = new();
  private CookiesController? _controller;
  private Mock<ILogger<CookiesController>>? _mockLogger;

  [TestInitialize]
  public void BeforeEachTest()
  {
    _mockContentService = new Mock<IContentService>();
    _mockLogger = new Mock<ILogger<CookiesController>>();
    
    _controller = new CookiesController(_mockLogger.Object, _mockContentService.Object);
  }

  [TestMethod]
  public async Task Index_NoContent_NavigatesToErrorPageAsync()
  {
    _mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync((CookiesPage)default!);
    _controller = new CookiesController(_mockLogger!.Object, _mockContentService.Object);

    var result = await _controller!.Index();

    result.Should().NotBeNull();
    result.Should().BeEquivalentTo(new RedirectToActionResult("Error", "Home", null));

    _mockLogger.VerifyError("No content for the cookies page");
  }

  [TestMethod]
  public async Task Index_ContentFound_ReturnsCorrectModel()
  {
    var expectedContent = new CookiesPage()
    {
      Heading = "Test Heading",
      BodyHtml = "<p> Test Body </p>",
      ButtonText = "Test Button Text",
      Options = new List<Option>
      {
       new Option()
       {
        Label = "Click Me!",
        Value = "test option 1"
       },
       new Option()
       {
        Label = "No Click Me!",
        Value = "test option 2"
       },
      }
    };

    _mockContentService.Setup(x => x.GetCookiesPage()).ReturnsAsync(expectedContent);

    _controller = new CookiesController(_mockLogger!.Object, _mockContentService.Object);

    var result = await _controller!.Index();

    result.Should().NotBeNull();
    result.Should()
          .BeAssignableTo<ViewResult>()
          .Which.Model
                .Should()
                .BeEquivalentTo(new CookiesPageModel()
                {
                  Heading = expectedContent.Heading,
                  BodyContent = expectedContent.BodyHtml,
                  ButtonText = expectedContent.ButtonText,
                  Options = new List<OptionModel>()
                  {
                    new OptionModel()
                    {
                      Label = expectedContent.Options[0].Label,
                      Value = expectedContent.Options[0].Value,
                    },
                    new OptionModel()
                    {
                      Label = expectedContent.Options[1].Label,
                      Value = expectedContent.Options[1].Value,
                    }
                  }
                });
  }
}
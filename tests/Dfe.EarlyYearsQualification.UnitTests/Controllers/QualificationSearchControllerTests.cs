using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationSearchControllerTests
{
    [TestMethod]
    public async Task Get_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        mockRepository
            .Setup(x =>
                       x.Get(It.IsAny<int?>(),
                             It.IsAny<int?>(),
                             It.IsAny<int?>(),
                             It.IsAny<string?>(),
                             It.IsAny<string?>()))
            .ReturnsAsync([]);

        var controller =
            new QualificationSearchController(mockLogger.Object,
                                              mockRepository.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(x => x.GetQualificationListPage())
                          .ReturnsAsync(new QualificationListPage
                                        {
                                            BackButton = new NavigationLink
                                                         {
                                                             DisplayText = "TEST",
                                                             Href = "/",
                                                             OpenInNewTab = false
                                                         },
                                            Header = "TEST"
                                        });

        var result = await controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [TestMethod]
    public async Task Get_NoContent_LogsAndRedirectsToError()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationSearchController(mockLogger.Object,
                                              mockRepository.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(x => x.GetQualificationListPage())
                          .ReturnsAsync(default(QualificationListPage));

        var result = await controller.Get();

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification list page");
    }

    [TestMethod]
    public void Refine_SaveQualificationName_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationSearchController(mockLogger.Object,
                                              mockRepository.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = controller.Refine("Test");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        mockUserJourneyCookieService.Verify(x => x.SetQualificationNameSearchCriteria("Test"),
                                            Times.Once);
    }

    [TestMethod]
    public void Refine_NullParam_RedirectsToGet()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationSearchController(mockLogger.Object,
                                              mockRepository.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = controller.Refine(null);

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Get");
        mockUserJourneyCookieService.Verify(x => x.SetQualificationNameSearchCriteria(string.Empty),
                                            Times.Once);
    }

    [TestMethod]
    public void Refine_InvalidModel_LogsWarning()
    {
        var mockLogger = new Mock<ILogger<QualificationSearchController>>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();

        var controller =
            new QualificationSearchController(mockLogger.Object,
                                              mockRepository.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        controller.ModelState.AddModelError("Key", "Error message");

        controller.Refine(null);

        mockLogger
            .VerifyWarning($"Invalid model state in {nameof(QualificationSearchController)} POST");
    }
}
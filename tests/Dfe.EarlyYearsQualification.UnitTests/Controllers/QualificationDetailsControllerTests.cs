using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QualificationDetailsControllerTests
{
    [TestMethod]
    public async Task Index_PassInNullQualificationId_ReturnsBadRequest()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IGovUkInsetTextRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller =
            new QualificationDetailsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object, mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = await controller.Index(string.Empty);

        result.Should().NotBeNull();

        var resultType = result as BadRequestResult;
        resultType.Should().NotBeNull();
        resultType!.StatusCode.Should().Be(400);
    }


    [TestMethod]
    public async Task Index_ContentServiceReturnsNullDetailsPage_RedirectsToHomeError()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IGovUkInsetTextRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller =
            new QualificationDetailsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object, mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        mockContentService.Setup(s => s.GetDetailsPage())
                          .ReturnsAsync((DetailsPage?)null);

        var result = await controller.Index("X");

        result.Should().BeOfType<RedirectToActionResult>();

        var actionResult = (RedirectToActionResult)result;

        actionResult.ActionName.Should().Be("Index");
        actionResult.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the qualification details page");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsNoQualification_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IGovUkInsetTextRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller =
            new QualificationDetailsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object, mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        const string qualificationId = "eyq-145";
        mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync((Qualification?)default);
        mockContentService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        var result = await controller.Index(qualificationId);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("Could not find details for qualification with ID: eyq-145");
    }

    [TestMethod]
    public async Task Index_ContentServiceReturnsQualification_ReturnsQualificationDetailsModel()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IGovUkInsetTextRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller =
            new QualificationDetailsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object, mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        const string qualificationId = "eyq-145";
        var qualificationResult = new Qualification(qualificationId, "Qualification Name", "NCFE", 2, "2014", "2019",
                                                    "ABC/547/900", "additonal requirements");
        mockContentService.Setup(x => x.GetQualificationById(qualificationId)).ReturnsAsync(qualificationResult);
        mockContentService.Setup(x => x.GetDetailsPage()).ReturnsAsync(new DetailsPage());
        var result = await controller.Index(qualificationId);

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
        model.AdditionalRequirements.Should().Be(qualificationResult.AdditionalRequirements);
    }

    [TestMethod]
    public void Get_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QualificationDetailsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IGovUkInsetTextRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        
        var controller =
            new QualificationDetailsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object, mockUserJourneyCookieService.Object)
            {
                ControllerContext = new ControllerContext
                                    {
                                        HttpContext = new DefaultHttpContext()
                                    }
            };

        var result = controller.Get();

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
}
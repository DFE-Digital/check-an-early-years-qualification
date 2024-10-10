using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AdviceControllerTests
{
    private static readonly Mock<IUserJourneyCookieService> UserJourneyMockNoOp = new();

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();
        var result = await controller.QualificationOutsideTheUnitedKingdom();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationOutsideTheUnitedKingdom();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task
        QualificationsStartedBetweenSept2014AndAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();
        var result = await controller.QualificationsStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task
        QualificationsStartedBetweenSept2014AndAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationsStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInNorthernIreland_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.QualificationsAchievedInNorthernIreland();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInNorthernIreland_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationsAchievedInNorthernIreland();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInScotland_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.QualificationsAchievedInScotland();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInScotland_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationsAchievedInScotland();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInWales_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInWales))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.QualificationsAchievedInWales();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInWales_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInWales))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationsAchievedInWales();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationLevel7_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationLevel7))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.QualificationLevel7();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationLevel7_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (level 7)";

        var advicePage = new AdvicePage
                         { Heading = "Heading (level 7)", Body = ContentfulContentHelper.Text("Anything") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationLevel7))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.QualificationLevel7();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(renderedHtmlBody);

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task Level6QualificationPre2014_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level6QualificationPre2014))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.Level6QualificationPre2014();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task Level6QualificationPre2014_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (level 6 pre 2014)";

        var advicePage = new AdvicePage
                         { Heading = "Heading (level 6 pre 2014)", Body = ContentfulContentHelper.Text("Anything") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level6QualificationPre2014))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.Level6QualificationPre2014();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(renderedHtmlBody);

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task Level6QualificationPost2014_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level6QualificationPost2014))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.Level6QualificationPost2014();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task Level6QualificationPost2014_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (level 6 post 2014)";

        var advicePage = new AdvicePage
                         { Heading = "Heading (level 6 post 2014)", Body = ContentfulContentHelper.Text("Anything") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level6QualificationPost2014))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.Level6QualificationPost2014();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(renderedHtmlBody);

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task PrivacyPolicy_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.TemporaryPrivacyPolicy))
                          .ReturnsAsync((AdvicePage?)default).Verifiable();

        var result = await controller.PrivacyPolicy();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task PrivacyPolicy_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (Privacy Policy)";

        var advicePage = new AdvicePage
                         { Heading = "Heading (Privacy Policy)", Body = ContentfulContentHelper.Text("Anything") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.TemporaryPrivacyPolicy))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.PrivacyPolicy();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(renderedHtmlBody);

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }
}
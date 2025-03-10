using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();
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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();
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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

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
    public async Task QualificationNotOnTheList_ContentServiceReturnsNullPage_GetsDefaultPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015)).ReturnsAsync(value: null);

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage
                         { Heading = "Default Heading", Body = ContentfulContentHelper.Text("Test html body") };
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

        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015), Times.Once);
        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsSpecificPage_GetsPageContent()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        var cannotFindQualificationPage = new CannotFindQualificationPage
                                          {
                                              Heading = "Specific cannot find page",
                                              Body = ContentfulContentHelper.Text("Test html body")
                                          };
        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015))
                          .ReturnsAsync(cannotFindQualificationPage);

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model!.Heading.Should().Be(cannotFindQualificationPage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockContentService.Verify(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList), Times.Never);
        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015), Times.Once);
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
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

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
    public async Task
        Level7QualificationStartedBetweenSept2014AndAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.Level7QualificationStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task
        Level7QualificationStartedBetweenSept2014AndAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (level 7 post 2014-2019)";

        var advicePage = new AdvicePage
                         {
                             Heading = "Heading (level 7 post 2014-2019)",
                             Body = ContentfulContentHelper.Text("Anything")
                         };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.Level7QualificationStartedBetweenSept2014AndAug2019();

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
    public async Task Level7QualificationAfterAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.Level7QualificationAfterAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task Level7QualificationAfterAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        const string renderedHtmlBody = "Test html body (level 7 post 2019)";

        var advicePage = new AdvicePage
                         { Heading = "Heading (level 7 post 2019)", Body = ContentfulContentHelper.Text("Anything") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync(renderedHtmlBody);

        var result = await controller.Level7QualificationAfterAug2019();

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
    public async Task Help_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Help))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();
        var result = await controller.Help();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task Help_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        var advicePage = new AdvicePage { Heading = "Heading", Body = ContentfulContentHelper.Text("Test html body") };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Help))
                          .ReturnsAsync(advicePage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.Help();

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
    public void OnActionExecuting_ClearsCookies()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object);

        controller.OnActionExecuting(null!);

        UserJourneyMockNoOp.Verify(o => o.SetUserSelectedQualificationFromList(YesOrNo.No), Times.Once);
        UserJourneyMockNoOp.Verify(o => o.ClearAdditionalQuestionsAnswers(), Times.Once);
    }
}
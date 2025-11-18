using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AdviceControllerTests
{
    private static readonly Mock<IUserJourneyCookieService> UserJourneyMockNoOp = new Mock<IUserJourneyCookieService>();
    private const string BodyContent = "Test html body";
    private const string Heading = "Heading";

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();
        var result = await controller.QualificationOutsideTheUnitedKingdom();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedOutsideTheUk))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationOutsideTheUnitedKingdom();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task
        QualificationsStartedBetweenSept2014AndAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();
        var result = await controller.QualificationsStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task
        QualificationsStartedBetweenSept2014AndAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationsStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInNorthernIreland_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.QualificationsAchievedInNorthernIreland();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInNorthernIreland_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInNorthernIreland))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationsAchievedInNorthernIreland();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be("Test html body");

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInScotland_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.QualificationsAchievedInScotland();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInScotland_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInScotland))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationsAchievedInScotland();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationsAchievedInWales_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInWales))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.QualificationsAchievedInWales();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationsAchievedInWales_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationsAchievedInWales))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationsAchievedInWales();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsNullPage_GetsDefaultPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetIsUserCheckingTheirOwnQualification()).Returns("no");
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015, false)).ReturnsAsync(value: null);

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015, false), Times.Once);
        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsSpecificPage_GetsPageContent()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetIsUserCheckingTheirOwnQualification()).Returns("no");
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        var cannotFindQualificationPage = new CannotFindQualificationPage
                                          {
                                              Heading = Heading,
                                              Body = ContentfulContentHelper.Text(BodyContent)
                                          };
        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015, false))
                          .ReturnsAsync(cannotFindQualificationPage);

        mockAdvicePageMapper.Setup(x => x.Map(cannotFindQualificationPage))
                            .ReturnsAsync(new QualificationNotOnListPageModel
                                          { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(cannotFindQualificationPage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockContentService.Verify(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList), Times.Never);
        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015, false), Times.Once);
        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<CannotFindQualificationPage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.QualificationNotOnTheList))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.QualificationNotOnTheList();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task
        Level7QualificationStartedBetweenSept2014AndAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.Level7QualificationStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task
        Level7QualificationStartedBetweenSept2014AndAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage
                         {
                             Heading = Heading,
                             Body = ContentfulContentHelper.Text(BodyContent)
                         };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationStartedBetweenSept2014AndAug2019))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel
                                          { Heading = advicePage.Heading, BodyContent = BodyContent });

        var result = await controller.Level7QualificationStartedBetweenSept2014AndAug2019();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task Level7QualificationAfterAug2019_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019))
                          .ReturnsAsync((AdvicePage?)null).Verifiable();

        var result = await controller.Level7QualificationAfterAug2019();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the advice page");
    }

    [TestMethod]
    public async Task Level7QualificationAfterAug2019_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var advicePage = new AdvicePage { Heading = Heading, Body = ContentfulContentHelper.Text(BodyContent) };
        mockContentService.Setup(x => x.GetAdvicePage(AdvicePages.Level7QualificationAfterAug2019))
                          .ReturnsAsync(advicePage);

        mockAdvicePageMapper.Setup(x => x.Map(advicePage))
                            .ReturnsAsync(new AdvicePageModel { Heading = Heading, BodyContent = BodyContent });

        var result = await controller.Level7QualificationAfterAug2019();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as AdvicePageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(advicePage.Heading);
        model.BodyContent.Should().Be(BodyContent);

        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public void OnActionExecuting_ClearsCookies()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        controller.OnActionExecuting(null!);

        UserJourneyMockNoOp.Verify(o => o.SetUserSelectedQualificationFromList(YesOrNo.No), Times.Once);
        UserJourneyMockNoOp.Verify(o => o.ClearAdditionalQuestionsAnswers(), Times.Once);
    }

    [TestMethod]
    public void Help_RedirectsToNewHelpJourney()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockAdvicePageMapper.Object);

        var result = controller.Help();
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");
    }
}
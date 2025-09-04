using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class AdviceControllerTests
{
    private static readonly Mock<IUserJourneyCookieService> UserJourneyMockNoOp = new();
    private const string BodyContent = "Test html body";
    private const string Heading = "Heading";

    [TestMethod]
    public async Task QualificationOutsideTheUnitedKingdom_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015)).ReturnsAsync(value: null);

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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

        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015), Times.Once);
        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<AdvicePage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsSpecificPage_GetsPageContent()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        UserJourneyMockNoOp.Setup(x => x.GetLevelOfQualification()).Returns(2);
        UserJourneyMockNoOp.Setup(x => x.GetWhenWasQualificationStarted()).Returns((2, 2015));

        var cannotFindQualificationPage = new CannotFindQualificationPage
                                          {
                                              Heading = Heading,
                                              Body = ContentfulContentHelper.Text(BodyContent)
                                          };
        mockContentService.Setup(x => x.GetCannotFindQualificationPage(2, 2, 2015))
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
        mockContentService.Verify(x => x.GetCannotFindQualificationPage(2, 2, 2015), Times.Once);
        mockAdvicePageMapper.Verify(x => x.Map(It.IsAny<CannotFindQualificationPage>()), Times.Once);
    }

    [TestMethod]
    public async Task QualificationNotOnTheList_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

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
    public async Task Help_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        mockContentService.Setup(x => x.GetHelpPage())
                          .ReturnsAsync((HelpPage?)null).Verifiable();
        var result = await controller.Help();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the help page");
    }

    [TestMethod]
    public async Task Help_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        var helpPage = new HelpPage { Heading = Heading };
        mockContentService.Setup(x => x.GetHelpPage())
                          .ReturnsAsync(helpPage);

        mockHelpPageMapper.Setup(x => x.Map(It.IsAny<HelpPage>(), It.IsAny<string>()))
                          .ReturnsAsync(new HelpPageModel { Heading = Heading, PostHeadingContent = BodyContent });

        var result = await controller.Help();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as HelpPageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(helpPage.Heading);
        model.PostHeadingContent.Should().Be(BodyContent);

        mockHelpPageMapper.Verify(x => x.Map(It.IsAny<HelpPage>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task Get_Post_ValidModelState_CallsNotificationService()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);
        controller.ModelState.Clear();

        const string emailAddress = "test@test.com";
        const string selectedOption = "I need help";
        const string message = "This is a test message";

        var result = await controller.Help(new HelpPageModel
                                           {
                                               EmailAddress = emailAddress, AdditionalInformationMessage = message,
                                               SelectedOption = selectedOption
                                           });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("HelpConfirmation");

        mockNotificationService.Verify(x => x.SendHelpPageNotification(It.IsAny<HelpPageNotification>()), Times.Once());
    }

    [TestMethod]
    public async Task Get_Post_InvalidModelState_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);
        var helpPage = new HelpPage { Heading = Heading };
        mockContentService.Setup(x => x.GetHelpPage())
                          .ReturnsAsync(helpPage);

        mockHelpPageMapper.Setup(x => x.Map(It.IsAny<HelpPage>(), It.IsAny<string>()))
                          .ReturnsAsync(new HelpPageModel { Heading = Heading, PostHeadingContent = BodyContent });

        controller.ModelState.AddModelError(nameof(HelpPageModel.EmailAddress), "Invalid");
        controller.ModelState.AddModelError(nameof(HelpPageModel.SelectedOption), "Invalid");
        controller.ModelState.AddModelError(nameof(HelpPageModel.AdditionalInformationMessage), "Invalid");

        var result = await controller.Help(new HelpPageModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();
        resultType.ViewName.Should().Be("Help");

        var model = resultType.Model as HelpPageModel;
        model.Should().NotBeNull();
        model.HasErrors.Should().BeTrue();
        model.HasEmailAddressError.Should().BeTrue();
        model.HasFurtherInformationError.Should().BeTrue();
        model.HasNoEnquiryOptionSelectedError.Should().BeTrue();
    }

    [TestMethod]
    public async Task HelpConfirmation_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        mockContentService.Setup(x => x.GetHelpConfirmationPage())
                          .ReturnsAsync((HelpConfirmationPage?)null).Verifiable();
        var result = await controller.HelpConfirmation();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the help confirmation page");
    }

    [TestMethod]
    public async Task HelpConfirmation_ContentServiceReturnsAdvicePage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        var helpConfirmationPage = new HelpConfirmationPage
                                   {
                                       SuccessMessage = "Success", BodyHeading = "Body heading",
                                       FeedbackComponent = new FeedbackComponent()
                                   };
        mockContentService.Setup(x => x.GetHelpConfirmationPage())
                          .ReturnsAsync(helpConfirmationPage);

        mockHelpConfirmationPageModelMapper.Setup(x => x.Map(It.IsAny<HelpConfirmationPage>()))
                                           .ReturnsAsync(new HelpConfirmationPageModel
                                                         {
                                                             BodyHeading = helpConfirmationPage.BodyHeading,
                                                             Body = BodyContent,
                                                             SuccessMessage = helpConfirmationPage.SuccessMessage
                                                         });

        var result = await controller.HelpConfirmation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as HelpConfirmationPageModel;
        model.Should().NotBeNull();

        model.SuccessMessage.Should().Be(helpConfirmationPage.SuccessMessage);
        model.BodyHeading.Should().Be(helpConfirmationPage.BodyHeading);
        model.Body.Should().Be("Test html body");

        mockHelpConfirmationPageModelMapper.Verify(x => x.Map(It.IsAny<HelpConfirmationPage>()), Times.Exactly(1));
    }

    [TestMethod]
    public void OnActionExecuting_ClearsCookies()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockAdvicePageMapper = new Mock<IAdvicePageMapper>();
        var mockHelpPageMapper = new Mock<IHelpPageMapper>();
        var mockHelpConfirmationPageModelMapper = new Mock<IHelpConfirmationPageModelMapper>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object,
                                              mockAdvicePageMapper.Object,
                                              mockHelpPageMapper.Object,
                                              mockHelpConfirmationPageModelMapper.Object);

        controller.OnActionExecuting(null!);

        UserJourneyMockNoOp.Verify(o => o.SetUserSelectedQualificationFromList(YesOrNo.No), Times.Once);
        UserJourneyMockNoOp.Verify(o => o.ClearAdditionalQuestionsAnswers(), Times.Once);
    }
}
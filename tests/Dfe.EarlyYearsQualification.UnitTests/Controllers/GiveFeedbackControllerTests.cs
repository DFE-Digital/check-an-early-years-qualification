using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.FeedbackForm;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class GiveFeedbackControllerTests
{
    [TestMethod]
    public async Task Get_ContentServiceReturnsNull_RedirectsToError()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync((FeedbackFormPage?)null);

        var result = await controller.Get();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Get_ContentServiceReturnsData_ReturnsExpectedView()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var feedbackFormPage = GetFeedbackFormPage();

        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync(feedbackFormPage);

        var result = await controller.Get();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();

        var modelData = resultType.Model as FeedbackFormPageModel;
        modelData.Should().NotBeNull();
        modelData.Heading.Should().Match(feedbackFormPage.Heading);
        
        mockFeedbackFormService.Verify(x => x.SetDefaultAnswers(It.IsAny<FeedbackFormPage>(), It.IsAny<FeedbackFormPageModel>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_ContentServiceReturnsNull_RedirectsToError()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync((FeedbackFormPage?)null);

        var model = GetFeedbackFormPageModel();

        var result = await controller.Post(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Post_ValidationContainsError_ReturnsGetView()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var feedbackFormPage = GetFeedbackFormPage();

        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync(feedbackFormPage);
        mockFeedbackFormService
            .Setup(x => x.ValidateQuestions(It.IsAny<FeedbackFormPage>(), It.IsAny<FeedbackFormPageModel>()))
            .Returns(new ErrorSummaryModel
                     {
                         ErrorSummaryLinks = [new ErrorSummaryLink()]
                     });

        var model = GetFeedbackFormPageModel();

        var result = await controller.Post(model);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Get");

        var modelData = resultType.Model as FeedbackFormPageModel;
        modelData.Should().NotBeNull();
        modelData.HasError.Should().BeTrue();
        modelData.ErrorSummaryModel.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Post_PassesValidation_RedirectsToConfirmation()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var feedbackFormPage = GetFeedbackFormPage();

        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync(feedbackFormPage);
        mockFeedbackFormService
            .Setup(x => x.ValidateQuestions(It.IsAny<FeedbackFormPage>(), It.IsAny<FeedbackFormPageModel>()))
            .Returns(new ErrorSummaryModel { ErrorSummaryLinks = [] });
        mockFeedbackFormService.Setup(x => x.ConvertQuestionListToString(It.IsAny<FeedbackFormPageModel>()))
                               .Returns("Message");

        var model = GetFeedbackFormPageModel();

        var result = await controller.Post(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Confirmation");

        mockNotificationService
            .Verify(x => x.SendEmbeddedFeedbackFormNotification(It.IsAny<EmbeddedFeedbackFormNotification>()),
                    Times.Once());
    }

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsNull_RedirectsToError()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        mockContentService.Setup(x => x.GetFeedbackFormConfirmationPage())
                          .ReturnsAsync((FeedbackFormConfirmationPage?)null);

        var result = await controller.Confirmation();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsData_ReturnsView()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var pageData = new FeedbackFormConfirmationPage
                       {
                           SuccessMessage = "Success"
                       };
        mockContentService.Setup(x => x.GetFeedbackFormConfirmationPage()).ReturnsAsync(pageData);
        mockUserJourneyCookieService.Setup(x => x.GetHasSubmittedEmailAddressInFeedbackFormQuestion()).Returns(true);

        var result = await controller.Confirmation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();

        var model = resultType.Model as FeedbackFormConfirmationPageModel;
        model.Should().NotBeNull();
        model.SuccessMessage.Should().Be(pageData.SuccessMessage);
        model.ShowOptionalSection.Should().BeTrue();
    }

    [TestMethod]
    public void HasUserGotWhatTheyNeededToday_PassInTrue_CallsCookieServiceWithCorrectValue()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var result = controller.HasUserGotWhatTheyNeededToday(true);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OkResult>();
        mockUserJourneyCookieService.Verify(x => x.SetHasUserGotEverythingTheyNeededToday("yes"), Times.Once());
    }
    
    [TestMethod]
    public void HasUserGotWhatTheyNeededToday_PassInFalse_CallsCookieServiceWithCorrectValue()
    {
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var controller = new GiveFeedbackController(mockContentService.Object, mockContentParser.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object);

        var result = controller.HasUserGotWhatTheyNeededToday(false);
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OkResult>();
        mockUserJourneyCookieService.Verify(x => x.SetHasUserGotEverythingTheyNeededToday("no"), Times.Once());
    }

    private static FeedbackFormPage GetFeedbackFormPage()
    {
        return new FeedbackFormPage
               {
                   Heading = "Heading",
                   CtaButtonText = "Continue",
                   ErrorBannerHeading = "Error",
                   BackButton = new NavigationLink()
               };
    }

    private static FeedbackFormPageModel GetFeedbackFormPageModel()
    {
        return new FeedbackFormPageModel
               {
                   Heading = "Heading",
                   CtaButtonText = "Continue",
                   ErrorBannerHeading = "Error"
               };
    }
}
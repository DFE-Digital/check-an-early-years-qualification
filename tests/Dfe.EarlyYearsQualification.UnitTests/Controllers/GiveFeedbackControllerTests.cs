using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
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
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

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
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        var feedbackFormPage = GetFeedbackFormPage();
        mockContentService.Setup(x => x.GetFeedbackFormPage()).ReturnsAsync(feedbackFormPage);

        var expectedModel = GetFeedbackFormPageModel();
        mockFeedbackFormPageMapper.Setup(x => x.Map(feedbackFormPage)).ReturnsAsync(expectedModel);

        var result = await controller.Get();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();

        var modelData = resultType.Model as FeedbackFormPageModel;
        modelData.Should().NotBeNull();
        modelData.Heading.Should().Match(feedbackFormPage.Heading);
    }

    [TestMethod]
    public async Task Post_WithAnswers_SendsNotificationAndRedirectsToConfirmation()
    {
        var mockContentService = new Mock<IContentService>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        mockFeedbackFormService.Setup(x => x.ConvertQuestionListToString(It.IsAny<FeedbackFormPageModel>(), It.IsAny<string>()))
                               .Returns("Message");

        var model = GetFeedbackFormPageModel();
        model.QuestionList =
        [
            new FeedbackFormQuestionListModel { Question = "Test Question", Answer = "Test Answer" }
        ];

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
    public async Task Post_WithNoAnswers_DoesNotSendNotificationButRedirectsToConfirmation()
    {
        var mockContentService = new Mock<IContentService>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        var model = GetFeedbackFormPageModel();
        model.QuestionList =
        [
            new FeedbackFormQuestionListModel { Question = "Test Question", Answer = null }
        ];

        var result = await controller.Post(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Confirmation");

        mockNotificationService
            .Verify(x => x.SendEmbeddedFeedbackFormNotification(It.IsAny<EmbeddedFeedbackFormNotification>()),
                    Times.Never());
    }

    [TestMethod]
    public async Task Post_WithEmptyQuestionList_DoesNotSendNotificationButRedirectsToConfirmation()
    {
        var mockContentService = new Mock<IContentService>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        var model = GetFeedbackFormPageModel();
        model.QuestionList = [];

        var result = await controller.Post(model);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Confirmation");

        mockNotificationService
            .Verify(x => x.SendEmbeddedFeedbackFormNotification(It.IsAny<EmbeddedFeedbackFormNotification>()),
                    Times.Never());
    }

    [TestMethod]
    public async Task Post_WithMixedAnswers_SendsNotificationAndRedirectsToConfirmation()
    {
        var mockContentService = new Mock<IContentService>();
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        mockFeedbackFormService.Setup(x => x.ConvertQuestionListToString(It.IsAny<FeedbackFormPageModel>(), It.IsAny<string>()))
                               .Returns("Message");

        var model = GetFeedbackFormPageModel();
        model.QuestionList =
        [
            new FeedbackFormQuestionListModel { Question = "Question 1", Answer = null },
            new FeedbackFormQuestionListModel { Question = "Question 2", Answer = "Some Answer" },
            new FeedbackFormQuestionListModel { Question = "Question 3", Answer = null }
        ];

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
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

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
        var mockFeedbackFormService = new Mock<IFeedbackFormService>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockNotificationService = new Mock<INotificationService>();
        var mockFeedbackFormPageMapper = new Mock<IFeedbackFormPageMapper>();
        var mockFeedbackFormConfirmationPageMapper = new Mock<IFeedbackFormConfirmationPageMapper>();
        var controller = new GiveFeedbackController(mockContentService.Object,
                                                    mockFeedbackFormService.Object, mockUserJourneyCookieService.Object,
                                                    mockNotificationService.Object,
                                                    mockFeedbackFormPageMapper.Object,
                                                    mockFeedbackFormConfirmationPageMapper.Object);

        var pageData = new FeedbackFormConfirmationPage
                       {
                           SuccessMessage = "Success"
                       };
        mockContentService.Setup(x => x.GetFeedbackFormConfirmationPage()).ReturnsAsync(pageData);
        mockUserJourneyCookieService.Setup(x => x.GetHasSubmittedEmailAddressInFeedbackFormQuestion()).Returns(true);

        var expectedModel = new FeedbackFormConfirmationPageModel { SuccessMessage = pageData.SuccessMessage };
        mockFeedbackFormConfirmationPageMapper.Setup(x => x.Map(It.IsAny<FeedbackFormConfirmationPage>()))
                                              .ReturnsAsync(expectedModel);
        
        var result = await controller.Confirmation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;

        resultType.Should().NotBeNull();

        var model = resultType.Model as FeedbackFormConfirmationPageModel;
        model.Should().NotBeNull();
        model.SuccessMessage.Should().Be(pageData.SuccessMessage);
        model.ShowOptionalSection.Should().BeTrue();
    }

    private static FeedbackFormPage GetFeedbackFormPage()
    {
        return new FeedbackFormPage
               {
                   Heading = "Heading",
                   CtaButtonText = "Continue",
                   BackButton = new NavigationLink()
               };
    }

    private static FeedbackFormPageModel GetFeedbackFormPageModel()
    {
        return new FeedbackFormPageModel
               {
                   Heading = "Heading",
                   CtaButtonText = "Continue",
               };
    }
}
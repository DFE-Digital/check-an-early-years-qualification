using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HelpControllerTests
{
    private static readonly Mock<IUserJourneyCookieService> UserJourneyMockNoOp = new();




/*
    [TestMethod]
    public async Task Help_ContentServiceReturnsNoAdvicePage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object);

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
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object, mockNotificationService.Object);

        var helpPage = new HelpPage { Heading = "Heading" };
        mockContentService.Setup(x => x.GetHelpPage())
                          .ReturnsAsync(helpPage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.Help();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as HelpPageModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(helpPage.Heading);
        model.PostHeadingContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task Get_Post_ValidModelState_CallsNotificationService()
    {
        var mockLogger = new Mock<ILogger<AdviceController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object, mockNotificationService.Object);
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
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object, mockNotificationService.Object);
        var helpPage = new HelpPage { Heading = "Heading" };
        mockContentService.Setup(x => x.GetHelpPage())
                          .ReturnsAsync(helpPage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

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
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object,
                                              mockContentService.Object,
                                              mockContentParser.Object,
                                              UserJourneyMockNoOp.Object,
                                              mockNotificationService.Object);

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
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockNotificationService = new Mock<INotificationService>();

        var controller = new AdviceController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                              UserJourneyMockNoOp.Object, mockNotificationService.Object);

        var helpConfirmationPage = new HelpConfirmationPage
                                   {
                                       SuccessMessage = "Success", BodyHeading = "Body heading",
                                       FeedbackComponent = new FeedbackComponent()
                                   };
        mockContentService.Setup(x => x.GetHelpConfirmationPage())
                          .ReturnsAsync(helpConfirmationPage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var result = await controller.HelpConfirmation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ConfirmationPageViewModel;
        model.Should().NotBeNull();

        model.SuccessMessage.Should().Be(helpConfirmationPage.SuccessMessage);
        model.BodyHeading.Should().Be(helpConfirmationPage.BodyHeading);
        model.Body.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Exactly(2));
    }*/
}
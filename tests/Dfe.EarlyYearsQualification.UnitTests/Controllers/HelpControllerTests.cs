using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.Extensions.Configuration;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HelpControllerTests
{
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IConfigurationSection> _configurationSectionMock = new();
    Mock<ILogger<HelpController>> mockLogger = new Mock<ILogger<HelpController>>();
    Mock<IContentService> mockContentService = new Mock<IContentService>();
    Mock<IGovUkContentParser> mockContentParser = new Mock<IGovUkContentParser>();
    Mock<INotificationService> mockNotificationService = new Mock<INotificationService>();
    Mock<IDateQuestionModelValidator> mockDateQuestionValidator = new Mock<IDateQuestionModelValidator>();
    Mock<IPlaceholderUpdater> mockPlaceHolderUpdater = new Mock<IPlaceholderUpdater>();
    Mock<IUserJourneyCookieService> mockUserJourneyService = new Mock<IUserJourneyCookieService>();

    private HelpController GetSut()
    {
        return new HelpController(mockLogger.Object,
                                mockContentService.Object,
                                mockContentParser.Object,
                                mockUserJourneyService.Object,
                                mockNotificationService.Object,
                                mockDateQuestionValidator.Object,
                                mockPlaceHolderUpdater.Object
                                );
    }

    // Get help tests

    [TestMethod]
    public async Task GetHelp_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        mockContentService.Setup(x => x.GetGetHelpPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().GetHelp();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("'Get help page' content could not be found");
    }

    [TestMethod]
    public async Task GetHelp_ContentServiceReturnsGetHelpPage_ReturnsGetHelpPageViewModel()
    {
        // Arrange
        var content = new GetHelpPage { Heading = "Heading" };

        mockContentService.Setup(x => x.GetGetHelpPage()).ReturnsAsync(content);
        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        // Act
        var result = await GetSut().GetHelp();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as GetHelpPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.PostHeadingContent.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    [DataRow("QuestionAboutAQualification", "QualificationDetails")]
    [DataRow("IssueWithTheService", "ProvideDetails")]
    public async Task Post_GetHelp_ValidModelStateRedirectsToExpectedPage(string selectedOption, string pageToRedirectTo)
    {
        // Arrange
        var content = new GetHelpPage
        {
           EnquiryReasons = new List<EnquiryOption>
           {
               new EnquiryOption
               {
                   Value = "QuestionAboutAQualification",
                   Label = "Question about a qualification"
               },
               new EnquiryOption
               {
                   Value = "IssueWithTheService",
                   Label = "Issue with the service"
               }
           }
        };
        
        mockContentService.Setup(x => x.GetGetHelpPage()).ReturnsAsync(content);

        var submittedViewModel = new GetHelpPageViewModel()
        {
            SelectedOption = selectedOption,
        };

        // Act
        var result = await GetSut().GetHelp(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be(pageToRedirectTo);

        mockUserJourneyService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_GetHelp_InvalidEnquiryOption_ReturnsGetHelpPageViewModel()
    {
        // Arrange
        var submission = new GetHelpPageViewModel()
        {
            SelectedOption = "invalid enquiry option",
            EnquiryReasons = new List<EnquiryOptionModel>()
            {
                new EnquiryOptionModel()
                {
                    Value = "QuestionAboutAQualification",
                    Label = "Question about a qualification"
                },
                new EnquiryOptionModel()
                {
                    Value = "IssueWithTheService",
                    Label = "Issue with the service"
                }
            }
        };

        // Act
        var result = await GetSut().GetHelp(submission);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Post_GetHelp_InvalidModelState_ReturnsGetHelpPageViewModel()
    {
        // Arrange
        mockContentService.Setup(x => x.GetGetHelpPage()).ReturnsAsync(new GetHelpPage());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(GetHelpPageViewModel.SelectedOption), "Invalid");

        // Act
        var result = await controller.GetHelp(new());

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as GetHelpPageViewModel;
        model.Should().NotBeNull();

        model.HasNoEnquiryOptionSelectedError.Should().BeTrue();
    }

    // Help qualification details tests

    [TestMethod]
    public async Task QualificationDetails_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("'Help qualification details page' content could not be found");
    }

    [TestMethod]
    public async Task QualificationDetails_ContentServiceReturnsHelpProvideDetailsPage_ReturnsProvideDetailsPageViewModel_PrepopulatedWithQualificationDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
            BackButton = new NavigationLink()
            {
                Href = "/get-help",
                DisplayText = "Back to get help"
            }
        };

        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = "Question about a qualification",
        };
        
        var startedAt = (1, 2000);
        var awardedAt = (6, 2002);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        mockUserJourneyService.Setup(x => x.GetAwardingOrganisation()).Returns("Awarding organisation");
        mockUserJourneyService.Setup(x => x.GetSelectedQualificationName()).Returns("Qualification name");

        mockUserJourneyService.Setup(x => x.GetWhenWasQualificationStarted()).Returns(startedAt);
        mockUserJourneyService.Setup(x => x.GetWhenWasQualificationAwarded()).Returns(awardedAt);

        // Act
        var result = await GetSut().QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.BackButton.DisplayText.Should().Be(content.BackButton.DisplayText);
        model.BackButton.Href.Should().Be(content.BackButton.Href);

        model.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(startedAt.Item1);
        model.QuestionModel.StartedQuestion.SelectedYear.Should().Be(startedAt.Item2);
        model.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(awardedAt.Item1);
        model.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(awardedAt.Item2);
    }

    [TestMethod]
    public async Task Post_QualificationDetails_ValidModelStateRedirectsToProvideDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };

        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = "Question about a qualification",
            }
        );

        var submittedViewModel = new QualificationDetailsPageViewModel()
        {
            QualificationName = "Qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2000
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = 5,
                    SelectedYear = 2003
                }
            },
            AwardingOrganisation = "Some organisation where the qualification is from",
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(submittedViewModel.QuestionModel, content)).Returns(
            new DatesValidationResult()
            {
                StartedValidationResult = new DateValidationResult()
                {
                    MonthValid = true,
                    YearValid = true
                },
                AwardedValidationResult = new DateValidationResult()
                {
                    MonthValid = true,
                    YearValid = true
                }
            }
        );

        // Act
        var result = await GetSut().QualificationDetails(submittedViewModel);

        var enquiry = mockUserJourneyService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be("Question about a qualification");

        enquiry.QualificationName.Should().Be("Qualification name");
        enquiry.QualificationStartDate.Should().Be("1/2000");
        enquiry.QualificationAwardedDate.Should().Be("5/2003");
        enquiry.AwardingOrganisation.Should().Be("Some organisation where the qualification is from");

        enquiry.AdditionalInformation.Should().BeNullOrEmpty();

        submittedViewModel.HasValidationErrors.Should().BeFalse();
        submittedViewModel.QuestionModel.HasErrors.Should().BeFalse();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be(nameof(HelpController.ProvideDetails));

        mockUserJourneyService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_QualificationDetails_OptionalStartDate_ValidModelStateRedirectsToProvideDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };

        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = "Question about a qualification",
            }
        );

        var submittedViewModel = new QualificationDetailsPageViewModel()
        {
            QualificationName = "Qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion =  null,
                AwardedQuestion = new()
                {
                    SelectedMonth = 5,
                    SelectedYear = 2003
                }
            },
            AwardingOrganisation = "Some organisation where the qualification is from",
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(submittedViewModel.QuestionModel, content)).Returns(
            new DatesValidationResult()
            {
                StartedValidationResult = null,
                AwardedValidationResult = new DateValidationResult()
                {
                    MonthValid = true,
                    YearValid = true
                }
            }
        );

        // Act
        var result = await GetSut().QualificationDetails(submittedViewModel);

        var enquiry = mockUserJourneyService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be("Question about a qualification");

        enquiry.QualificationName.Should().Be("Qualification name");
        enquiry.QualificationStartDate.Should().BeNullOrEmpty();
        enquiry.QualificationAwardedDate.Should().Be("5/2003");
        enquiry.AwardingOrganisation.Should().Be("Some organisation where the qualification is from");

        enquiry.AdditionalInformation.Should().BeNullOrEmpty();

        submittedViewModel.HasValidationErrors.Should().BeFalse();
        submittedViewModel.QuestionModel.HasErrors.Should().BeFalse();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be(nameof(HelpController.ProvideDetails));

        mockUserJourneyService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_QualificationDetails_MissingRequiredFieldQualificationName_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        SetUpQualificationDetailsContent();

        var vm = SetUpSubmission(null, "awarding org", 1, 2001, 1 , 2002, new(), new());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QualificationName), "Invalid");

        // Act
        var result = await controller.QualificationDetails(vm);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();

        model.HasQualificationNameError.Should().BeTrue();
    }

    [TestMethod]
    public async Task Post_QualificationDetails_MissingRequiredFieldAwardingOrganisation_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        SetUpQualificationDetailsContent();

        var vm = SetUpSubmission("qualification name", null, 1, 2001, 1, 2002, new(), new());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.AwardingOrganisation), "Invalid");

        // Act
        var result = await controller.QualificationDetails(vm);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();

        model.HasAwardingOrganisationError.Should().BeTrue();
    }

    [TestMethod]
    public async Task Post_QualificationDetails_MissingRequiredFieldAwardedMonth_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        SetUpQualificationDetailsContent();

        var vm = SetUpSubmission("qualification name", "awarding org", 1, 2001, null, 2002, new(), new() { MonthValid = false });

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QuestionModel.AwardedQuestion.SelectedMonth), "Invalid");

        // Act
        var result = await controller.QualificationDetails(vm);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();

        model.QuestionModel.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task Post_QualificationDetails_MissingRequiredFieldAwardeYear_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        SetUpQualificationDetailsContent();

        var vm = SetUpSubmission("qualification name", "awarding org", 1, 2001, 1, null, new(), new() { MonthValid = false });

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QuestionModel.AwardedQuestion.SelectedYear), "Invalid");

        // Act
        var result = await controller.QualificationDetails(vm);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();

        model.QuestionModel.HasErrors.Should().BeTrue();
    }

    // Help Provide Details Tests

    [TestMethod]
    public async Task ProvideDetails_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        mockContentService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().ProvideDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("'Help provide details page' content could not be found");
    }

    [TestMethod]
    [DataRow("Question about a qualification", "QualificationDetails")]
    [DataRow("Issue with the service", "GetHelp")]
    public async Task ProvideDetails_ContentServiceReturnsHelpProvideDetailsPage_ReturnsProvideDetailsPageViewModel(string selectedOption, string pageToRedirectTo)
    {
        // Arrange
        var content = new HelpProvideDetailsPage 
        { 
            Heading = "Heading",
            BackButtonToGetHelpPage = new NavigationLink() 
            {
                Href = "/get-help", 
                DisplayText = "Back to get help" 
            },
            BackButtonToQualificationDetailsPage = new NavigationLink()
            {
                Href = "/qualification-details",
                DisplayText = "Back to qualification details page"
            }
        };

        mockContentService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(content);

        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = selectedOption,
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        // Act
        var result = await GetSut().ProvideDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ProvideDetailsPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);

        if (pageToRedirectTo == "GetHelp")
        {
            model.BackButton.DisplayText.Should().Be(content.BackButtonToGetHelpPage.DisplayText);
            model.BackButton.Href.Should().Be(content.BackButtonToGetHelpPage.Href);
        }

        if (pageToRedirectTo == "QualificationDetails")
        {
            model.BackButton.DisplayText.Should().Be(content.BackButtonToQualificationDetailsPage.DisplayText);
            model.BackButton.Href.Should().Be(content.BackButtonToQualificationDetailsPage.Href);
        }
    }

    [TestMethod]
    public async Task Post_ProvideDetails_ValidModelStateRedirectsToEmailAddress()
    {
        // Arrange
        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = "Issue with the service",
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var submittedViewModel = new ProvideDetailsPageViewModel()
        {
            ProvideAdditionalInformation = "Some details about the issue",
        };

        // Act
        var result = await GetSut().ProvideDetails(submittedViewModel);

        var enquiry = mockUserJourneyService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be("Issue with the service");
        enquiry.AdditionalInformation.Should().Be("Some details about the issue");

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("EmailAddress");

        mockUserJourneyService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_ProvideDetails_InvalidModelState_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = "Issue with the service",
            }
        );

        mockContentService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(new HelpProvideDetailsPage());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(ProvideDetailsPageViewModel.ProvideAdditionalInformation), "Invalid");

        // Act
        var result = await controller.ProvideDetails(new());

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ProvideDetailsPageViewModel;
        model.Should().NotBeNull();

        model.HasAdditionalInformationError.Should().BeTrue();
    }

    // Help email address tests

    [TestMethod]
    public async Task EmailAddress_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        mockContentService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().EmailAddress();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("'Help email address page' content could not be found");
    }

    [TestMethod]
    public async Task EmailAddress_ContentServiceReturnsHelpEmailAddressPage_ReturnsEmailAddressPageViewModel()
    {
        // Arrange
        var content = new HelpEmailAddressPage
        {
            Heading = "Heading",
        };

        mockContentService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(content);

        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = "Question about a qualification",
            AdditionalInformation = "Some details about the issue",
            QualificationName = "A qualification name",
            QualificationStartDate = "1/2000",
            QualificationAwardedDate = "1/2001",
            AwardingOrganisation = "An awarding organisation",
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        // Act
        var result = await GetSut().EmailAddress();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as EmailAddressPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
    }

    [TestMethod]
    public async Task Post_EmailAddress_ValidModelStateRedirectsToHelpConfirmation()
    {
        // Arrange
        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = "Issue with the service",
            AdditionalInformation = "Some details about the issue",
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var submittedViewModel = new EmailAddressPageViewModel()
        {
            EmailAddress = "test@test.com"
        };

        // Act
        var result = await GetSut().EmailAddress(submittedViewModel);


        var enquiry = mockUserJourneyService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be("Issue with the service");
        enquiry.AdditionalInformation.Should().Be("Some details about the issue");

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Confirmation");

        mockNotificationService.Verify(x => x.SendHelpPageNotification(It.IsAny<HelpPageNotification>()), Times.Once());

        mockUserJourneyService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_EmailAddress_InvalidModelState_ReturnsEmailAddressPageViewModel()
    {
        // Arrange
        mockContentService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(new HelpEmailAddressPage());

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(ProvideDetailsPageViewModel.ProvideAdditionalInformation), "Invalid");

        // Act
        var result = await controller.EmailAddress(new());

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as EmailAddressPageViewModel;
        model.Should().NotBeNull();
        model.HasEmailAddressError.Should().BeTrue();
    }

    // Help confirmation tests

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        mockContentService.Setup(x => x.GetHelpConfirmationPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().Confirmation();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("'Help confirmation page' content could not be found");
    }

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsHelpConfirmationPage_ReturnsConfirmationPageViewModel()
    {
        // Arrange
        var content = new HelpConfirmationPage
        {
            SuccessMessage = "Message sent",
            FeedbackComponent= new FeedbackComponent() 
            { 
                Body = ContentfulContentHelper.Text("Feedback body"), 
                Header = "Feedback heading" 
            }
        };

        mockContentService.Setup(x => x.GetHelpConfirmationPage()).ReturnsAsync(content);

        // Act
        var result = await GetSut().Confirmation();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ConfirmationPageViewModel;
        model.Should().NotBeNull();

        model.SuccessMessage.Should().Be(content.SuccessMessage);
    }

    #region private methods for setup

    private void SetUpQualificationDetailsContent()
    {
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };
        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = "Question about a qualification",
            }
        );
    }

    private QualificationDetailsPageViewModel SetUpSubmission(string? qualificationName, string? awardingOrganisation,
        int? startedMonth, int? startedYear, int? awardedMonth, int? awardedYear, DateValidationResult startedValidationResult, DateValidationResult awardedValidationResult)
    {
        var vm = new QualificationDetailsPageViewModel()
        {
            QualificationName = qualificationName,
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = startedMonth,
                    SelectedYear = startedYear
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = awardedMonth,
                    SelectedYear = awardedYear
                },
            },
            AwardingOrganisation = awardingOrganisation,
        };


        mockDateQuestionValidator.Setup(x => x.IsValid(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>())).Returns(
            new DatesValidationResult()
            {
                StartedValidationResult = startedValidationResult,
                AwardedValidationResult = awardedValidationResult
            }
        );

        return vm;
    }

    #endregion
}
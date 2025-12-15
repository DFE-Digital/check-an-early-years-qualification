using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.UnitTests.Services;

[TestClass]
public class HelpServiceTests
{
    private Mock<ILogger<HelpService>> _mockLogger = new Mock<ILogger<HelpService>>();
    private Mock<IContentService> _mockContentService = new Mock<IContentService>();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
    private Mock<INotificationService> _mockNotificationService = new Mock<INotificationService>();
    private Mock<IDateQuestionModelValidator> _mockDateQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
    private Mock<IHelpGetHelpPageMapper> _mockHelpGetHelpPageMapper = new Mock<IHelpGetHelpPageMapper>();
    private Mock<IHelpQualificationDetailsPageMapper> _mockHelpQualificationDetailsPageMapper = new Mock<IHelpQualificationDetailsPageMapper>();
    private Mock<IHelpProvideDetailsPageMapper> _mockHelpProvideDetailsPageMapper = new Mock<IHelpProvideDetailsPageMapper>();
    private Mock<IHelpEmailAddressPageMapper> _mockHelpEmailAddressPageMapper = new Mock<IHelpEmailAddressPageMapper>();
    private Mock<IHelpConfirmationPageMapper> _mockHelpConfirmationPageMapper = new Mock<IHelpConfirmationPageMapper>();

    [TestMethod]
    public async Task GetGetHelpPageAsync_Calls_ContentService_GetGetHelpPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetGetHelpPageAsync();

        // Assert
        _mockContentService.Verify(o => o.GetGetHelpPage(), Times.Once);
    }

    [TestMethod]
    public async Task MapGetHelpPageContentToViewModelAsync_Calls_HelpGetHelpPageMapper_MapGetHelpPageContentToViewModelAsync()
    {
        // Arrange
        var content = new GetHelpPage();

        // Act
        var viewModel = await GetSut().MapGetHelpPageContentToViewModelAsync(content);

        // Assert
        _mockHelpGetHelpPageMapper.Verify(o => o.MapGetHelpPageContentToViewModelAsync(content), Times.Once);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.QuestionAboutAQualification, nameof(HelpFormEnquiryReasons.QuestionAboutAQualification))]
    [DataRow(HelpFormEnquiryReasons.IssueWithTheService, nameof(HelpFormEnquiryReasons.IssueWithTheService))]
    [DataRow(null, "")]
    public void GetSelectedOption_Returns_PreviouslySelectedRadioOption(string input, string expected)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry
            {
                ReasonForEnquiring = input
            }
        );

        // Act
        var result = GetSut().GetSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [TestMethod]
    public void GetSelectedOption_EnquiryIsNull_Returns_EmptyString()
    {
        // Act
        var result = GetSut().GetSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(nameof(HelpFormEnquiryReasons.IssueWithTheService), true)]
    [DataRow(nameof(HelpFormEnquiryReasons.QuestionAboutAQualification), true)]
    [DataRow("random value", false)]
    public void SelectedOptionIsValid_Returns_Expected(string input, bool expected)
    {
        // Arrange
        var content = new GetHelpPage
                      {
                          EnquiryReasons = new List<EnquiryOption>
                                           {
                                               new EnquiryOption
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.QuestionAboutAQualification),
                                                   Label = HelpFormEnquiryReasons.QuestionAboutAQualification
                                               },
                                               new EnquiryOption
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.IssueWithTheService),
                                                   Label = HelpFormEnquiryReasons.IssueWithTheService
                                               }
                                           }
                      };

        var viewModel = new GetHelpPageViewModel
                        {
                            SelectedOption = input
                        };

        // Act
        var result = GetSut().SelectedOptionIsValid(content, viewModel);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    [DataRow(nameof(HelpFormEnquiryReasons.QuestionAboutAQualification), nameof(HelpController.QualificationDetails))]
    [DataRow(nameof(HelpFormEnquiryReasons.IssueWithTheService), nameof(HelpController.ProvideDetails))]
    public void SetHelpFormEnquiryReason_Returns_Expected(string input, string controllerActionToRedirectTo)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var viewModel = new GetHelpPageViewModel
        {
            SelectedOption = input
        };

        // Act
        var result = GetSut().SetHelpFormEnquiryReason(viewModel);

        // Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be(controllerActionToRedirectTo);

        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public void SetHelpFormEnquiryReason_InvalidEnquiryReason_Returns_Expected()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var viewModel = new GetHelpPageViewModel
        {
            SelectedOption = "invalid value"
        };

        // Act
        var result = GetSut().SetHelpFormEnquiryReason(viewModel);

        // Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be("Index");
        result.ControllerName.Should().Be("Error");

        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Never);
    }

    [TestMethod]
    public void SetHelpFormEnquiryReason_Null_GetHelpFormEnquiry_InitialisesNew_Returns_Expected()
    {
        // Arrange
        var result = GetSut().SetHelpFormEnquiryReason(
            new GetHelpPageViewModel
            {
                SelectedOption = nameof(HelpFormEnquiryReasons.IssueWithTheService)
            }
        );

        // Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(HelpController.ProvideDetails));
      
        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpQualificationDetailsPageAsync_Calls_ContentService_GetHelpQualificationDetailsPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpQualificationDetailsPageAsync();

        // Assert
        _mockContentService.Verify(o => o.GetHelpQualificationDetailsPage(), Times.Once);
    }

    [TestMethod]
    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie_SetsExpectedViewModelValues()
    {
        // Arrange
        var viewModel = new QualificationDetailsPageViewModel();

        var enquiry = new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(enquiry);
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((1, 2000));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((2, 2002));

        // Act
        GetSut().SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel);

        // Assert
        viewModel.Should().NotBeNull();

        viewModel.QuestionModel.StartedQuestion.Should().NotBeNull();
        viewModel.QuestionModel.AwardedQuestion.Should().NotBeNull();

        viewModel.QualificationName.Should().Be(enquiry.QualificationName);
        viewModel.AwardingOrganisation.Should().Be(enquiry.AwardingOrganisation);
        viewModel.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(1);
        viewModel.QuestionModel.StartedQuestion.SelectedYear.Should().Be(2000);
        viewModel.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(2);
        viewModel.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(2002);
    }

    [TestMethod]
    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie_Overwrites_GetWhenWasQualificationStartedAndAwarded()
    {
        // Arrange
        var viewModel = new QualificationDetailsPageViewModel();

        var enquiry = new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
            QualificationStartDate = "5/2004",
            QualificationAwardedDate = "7/2008"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(enquiry);
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationStarted()).Returns((1, 2000));
        _mockUserJourneyCookieService.Setup(o => o.GetWhenWasQualificationAwarded()).Returns((2, 2002));

        // Act
        GetSut().SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel);

        // Assert
        viewModel.Should().NotBeNull();

        viewModel.QuestionModel.StartedQuestion.Should().NotBeNull();
        viewModel.QuestionModel.AwardedQuestion.Should().NotBeNull();

        viewModel.QualificationName.Should().Be(enquiry.QualificationName);
        viewModel.AwardingOrganisation.Should().Be(enquiry.AwardingOrganisation);
        viewModel.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(5);
        viewModel.QuestionModel.StartedQuestion.SelectedYear.Should().Be(2004);
        viewModel.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(7);
        viewModel.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(2008);
    }

    [TestMethod]
    public void MapHelpQualificationDetailsPageContentToViewModel_Calls_HelpQualificationDetailsPageMapper_MapGetHelpPageContentToViewModelAsync()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage();
        var viewModel = new QualificationDetailsPageViewModel();
        var datesValidationResult = new DatesValidationResult();
        var modelState = new ModelStateDictionary();

        // Act
        var result = GetSut().MapHelpQualificationDetailsPageContentToViewModel(viewModel, content, datesValidationResult, modelState);

        // Assert
        _mockHelpQualificationDetailsPageMapper.Verify(o => o.MapQualificationDetailsContentToViewModel(viewModel, content, datesValidationResult, modelState), Times.Once);
    }

    [TestMethod]
    public void SetHelpQualificationDetailsInCookie_Updates_EnquiryValues()
    {
        // Arrange
        var enquiry = new HelpFormEnquiry
                      {
            ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
        };

        var viewModel = new QualificationDetailsPageViewModel
                        {
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
            QuestionModel = new()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 2,
                    SelectedYear = 2003
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = 8,
                    SelectedYear = 2004
                }
            }
        };

        // Act
        GetSut().SetHelpQualificationDetailsInCookie(enquiry, viewModel);

        // Assert
        enquiry.QualificationStartDate.Should().Be("2/2003");
        enquiry.QualificationAwardedDate.Should().Be("8/2004");
        enquiry.QualificationName.Should().Be("Test Qualification Name");
        enquiry.AwardingOrganisation.Should().Be("Test Awarding Organisation");

        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(enquiry), Times.Once);
    }

    [TestMethod]
    [DataRow(true, true, true, true, false)]
    [DataRow(false, true, true, true, true)]
    [DataRow(true, false, true, true, true)]
    [DataRow(true, true, false, true, true)]
    [DataRow(true, true, true, false, true)]
    public void HasInvalidDates_Returns_Expected(bool sMonthValid, bool sYearValid, bool aMonthValid, bool aYearValid, bool expected)
    {
        // Arrange
        var validationResult = new DatesValidationResult
        {
            StartedValidationResult = new() { MonthValid = sMonthValid, YearValid = sYearValid },
            AwardedValidationResult = new() { MonthValid = aMonthValid, YearValid = aYearValid },
        };

        // Act
        var result = GetSut().HasInvalidDates(validationResult);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    public void ValidateDates_Calls_QuestionModelValidator_IsValid()
    {
        // Arrange
        var questionModel = new DatesQuestionModel();
        var content = new HelpQualificationDetailsPage();

        // Act
        var result = GetSut().ValidateDates(questionModel, content);

        // Assert
        _mockDateQuestionModelValidator.Verify(o => o.IsValid(questionModel, content), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_Calls_ContentService_GetHelpProvideDetailsPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpProvideDetailsPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpProvideDetailsPage(), Times.Once);
    }

    [TestMethod]
    public void MapProvideDetailsPageContentToViewModel_Calls_HelpProvideDetailsPageMapper_MapProvideDetailsPageContentToViewModel()
    {
        // Arrange
        var content = new HelpProvideDetailsPage();

        // Act
        var result = GetSut().MapProvideDetailsPageContentToViewModel(content, HelpFormEnquiryReasons.IssueWithTheService);

        // Assert
        _mockHelpProvideDetailsPageMapper.Verify(o => o.MapProvideDetailsPageContentToViewModel(content, HelpFormEnquiryReasons.IssueWithTheService), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_Calls_ContentService_GetHelpEmailAddressPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpEmailAddressPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpEmailAddressPage(), Times.Once);
    }

    [TestMethod]
    public void MapEmailAddressPageContentToViewModel_Calls_HelpEmailAddressPageMapper_HelpEmailAddressPageMapper_MapEmailAddressPageContentToViewModel()
    {
        // Arrange
        var content = new HelpEmailAddressPage();

        // Act
        var result = GetSut().MapEmailAddressPageContentToViewModel(content);

        // Assert
        _mockHelpEmailAddressPageMapper.Verify(o => o.MapEmailAddressPageContentToViewModel(content), Times.Once);
    }

    [TestMethod]
    public void SendHelpPageNotification_Calls_NotificationService_SendHelpPageNotification()
    {
        // Arrange
        var enquiry = new HelpPageNotification("test@test.com", new HelpFormEnquiry());

        // Act
        GetSut().SendHelpPageNotification(enquiry);

        // Assert
        _mockNotificationService.Verify(o => o.SendHelpPageNotification(enquiry), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_Calls_ContentService_GetHelpConfirmationPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpConfirmationPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpConfirmationPage(), Times.Once);
    }

    [TestMethod]
    public void MapConfirmationPageContentToViewModelAsync_Calls_HelpConfirmationPageMapper_MapConfirmationPageContentToViewModelAsync()
    {
        // Arrange
        var content = new HelpConfirmationPage();

        // Act
        var result = GetSut().MapConfirmationPageContentToViewModelAsync(content);

        // Assert
        _mockHelpConfirmationPageMapper.Verify(o => o.MapConfirmationPageContentToViewModelAsync(content), Times.Once);
    }

    [TestMethod]
    public void GetHelpFormEnquiry_Calls_JourneyCookieService_GetHelpFormEnquiry()
    {
        // Act
        _ = GetSut().GetHelpFormEnquiry();

        // Assert
        _mockUserJourneyCookieService.Verify(o => o.GetHelpFormEnquiry(), Times.Once);
    }

    [TestMethod]
    public void SetHelpFormEnquiry_Calls_JourneyCookieService_SetHelpFormEnquiry()
    {
        // Arrange
        var enquiry = new HelpFormEnquiry();

        // Act
        GetSut().SetHelpFormEnquiry(enquiry);

        // Assert
        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(enquiry), Times.Once);
    }

    private HelpService GetSut()
    {
        return new HelpService(
                                _mockLogger.Object,
                                _mockContentService.Object,
                                _mockUserJourneyCookieService.Object,
                                _mockNotificationService.Object,
                                _mockDateQuestionModelValidator.Object,
                                _mockHelpGetHelpPageMapper.Object,
                                _mockHelpQualificationDetailsPageMapper.Object,
                                _mockHelpProvideDetailsPageMapper.Object,
                                _mockHelpEmailAddressPageMapper.Object,
                                _mockHelpConfirmationPageMapper.Object
                                );
    }
}
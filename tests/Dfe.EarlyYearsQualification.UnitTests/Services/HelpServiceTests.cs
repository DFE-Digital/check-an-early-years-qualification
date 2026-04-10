using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
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
    private Mock<IContentService> _mockContentService = new();
    private Mock<IUserJourneyCookieService> _mockUserJourneyCookieService = new();
    private Mock<INotificationService> _mockNotificationService = new();
    private Mock<IDateQuestionModelValidator> _mockDateQuestionModelValidator = new();
    private Mock<IRadioQuestionHelpPageMapper> _mockHelpRadioQuestionHelpPageMapper = new();
    private Mock<IHelpQualificationDetailsPageMapper> _mockHelpQualificationDetailsPageMapper = new();
    private Mock<IHelpProvideDetailsPageMapper> _mockHelpProvideDetailsPageMapper = new();
    private Mock<IHelpEmailAddressPageMapper> _mockHelpEmailAddressPageMapper = new();
    private Mock<IHelpConfirmationPageMapper> _mockHelpConfirmationPageMapper = new();
    private Mock<IStaticPageMapper> _mockStaticPageMapper = new();

    [TestMethod]
    public async Task GetRadioQuestionHelpPageAsync_Calls_ContentService_GetRadioQuestionHelpPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetRadioQuestionHelpPageAsync(It.IsAny<string>());

        // Assert
        _mockContentService.Verify(o => o.GetRadioQuestionHelpPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task
        MapRadioQuestionHelpPageContentToViewModelAsync_Calls_HelpGetHelpPageMapper_MapRadioQuestionHelpPageContentToViewModelAsync()
    {
        // Arrange
        var content = new RadioQuestionHelpPage();

        // Act
        await GetSut().MapRadioQuestionHelpPageContentToViewModelAsync(content);

        // Assert
        _mockHelpRadioQuestionHelpPageMapper.Verify(o => o.MapRadioQuestionHelpPageContentToViewModelAsync(content), Times.Once);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript, nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript))]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs, nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs))]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol, nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol))]
    [DataRow(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification, nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification))]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IssueWithTheService, nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService))]
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
        var result = GetSut().GetWhyAreYouContactingUsSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [TestMethod]
    public void GetSelectedOption_EnquiryIsNull_Returns_EmptyString()
    {
        // Act
        var result = GetSut().GetWhyAreYouContactingUsSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService, nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService))]
    [DataRow(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam, nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam))]
    [DataRow(null, "")]
    public void GetWhatDoYouWantToDoNextSelectedOption_Returns_PreviouslySelectedRadioOption(string input, string expected)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(
             new HelpFormEnquiry
             {
                 WhatDoYouWantToDoNext = input
             }
        );

        // Act
        var result = GetSut().GetWhatDoYouWantToDoNextSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [TestMethod]
    public void GetWhatDoYouWantToDoNextSelectedOption_EnquiryIsNull_Returns_EmptyString()
    {
        // Act
        var result = GetSut().GetWhatDoYouWantToDoNextSelectedOption();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(string.Empty);
    }

    [TestMethod]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService), true)]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification), true)]
    [DataRow("random value", false)]
    public void SelectedOptionIsValid_Returns_Expected(string input, bool expected)
    {
        // Arrange
        var content = new RadioQuestionHelpPage
                      {
                          Options = new List<Option>
                                           {
                                               new Option
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification),
                                                   Label = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification
                                               },
                                               new Option
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService),
                                                   Label = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
                                               }
                                           }
                      };

        var viewModel = new RadioQuestionHelpPageViewModel
                        {
                            SelectedOption = input
                        };

        // Act
        var result = GetSut().SelectedOptionIsValid(content.Options, viewModel.SelectedOption);

        // Assert
        result.Should().Be(expected);
    }

    [TestMethod]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript), HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript)]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs), HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs)]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol), HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol)]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification), HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification)]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService), HelpFormEnquiryReasons.GetHelp.IssueWithTheService)]
    [DataRow("invalid option", "")]
    public void SetHelpFormEnquiryReason_Returns_Expected(string input, string expected)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var viewModel = new RadioQuestionHelpPageViewModel
                        {
                            SelectedOption = input
                        };

        // Act
        GetSut().SetHelpFormEnquiryReason(viewModel.SelectedOption);

        var result = GetSut().GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();
        result.ReasonForEnquiring.Should().Be(expected);

        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task GetStaticPage_Calls_ContentService_GetStaticPage()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.GetStaticPage(It.IsAny<string>());

        // Assert
        _mockContentService.Verify(o => o.GetStaticPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task MapStaticPage_Calls_StaticPageMapper_Map()
    {
        // Arrange
        var sut = GetSut();

        // Act
        _ = await sut.MapStaticPage(It.IsAny<StaticPage>());

        // Assert
        _mockStaticPageMapper.Verify(o => o.Map(It.IsAny<StaticPage>()), Times.Once);
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
                          ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
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
    public void
        SetAnyPreviouslyEnteredQualificationDetailsFromCookie_Overwrites_GetWhenWasQualificationStartedAndAwarded()
    {
        // Arrange
        var viewModel = new QualificationDetailsPageViewModel();

        var enquiry = new HelpFormEnquiry
                      {
                          ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
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
    public void
        MapHelpQualificationDetailsPageContentToViewModel_Calls_HelpQualificationDetailsPageMapper_MapRadioQuestionHelpPageContentToViewModelAsync()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage();
        var viewModel = new QualificationDetailsPageViewModel();
        var datesValidationResult = new DatesValidationResult();
        var modelState = new ModelStateDictionary();

        // Act
        GetSut()
            .MapHelpQualificationDetailsPageContentToViewModel(viewModel, content, datesValidationResult, modelState);

        // Assert
        _mockHelpQualificationDetailsPageMapper.Verify(o =>
                                                           o.MapQualificationDetailsContentToViewModel(viewModel,
                                                            content, datesValidationResult, modelState),
                                                       Times.Once);
    }

    [TestMethod]
    public void SetHelpQualificationDetailsInCookie_Updates_EnquiryValues()
    {
        // Arrange
        var enquiry = new HelpFormEnquiry
                      {
                          ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
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
    public void HasInvalidDates_Returns_Expected(bool sMonthValid, bool sYearValid, bool aMonthValid, bool aYearValid,
                                                 bool expected)
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
        GetSut().ValidateDates(questionModel, content);

        // Assert
        _mockDateQuestionModelValidator.Verify(o => o.IsValid(questionModel, content), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_Calls_ContentService_GetHelpProvideDetailsPage()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpProvideDetailsPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpProvideDetailsPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_ReasonIsIssueWithTheService_CallsContentServiceWithTechnicalIssueEntryId()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpProvideDetailsPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpProvideDetailsPage(HelpPages.TechnicalIssueProvideDetails), Times.Once);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol)]
    [DataRow(null)]
    [DataRow("")]
    public async Task GetHelpProvideDetailsPage_ReasonIsNotIssueWithTheService_CallsContentServiceWithHowCanWeHelpYouEntryId(string reason)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = reason
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpProvideDetailsPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpProvideDetailsPage(HelpPages.HowCanWeHelpYouProvideDetails), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_ContentServiceReturnsContent_ReturnsContent()
    {
        // Arrange
        var expectedContent = new HelpProvideDetailsPage
        {
            Heading = "Test Heading",
            CtaButtonText = "Continue"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpProvideDetailsPage(It.IsAny<string>()))
                           .ReturnsAsync(expectedContent);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpProvideDetailsPage();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedContent);
    }

    [TestMethod]
    public async Task GetHelpProvideDetailsPage_ContentServiceReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpProvideDetailsPage(It.IsAny<string>()))
                           .ReturnsAsync((HelpProvideDetailsPage?)null);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpProvideDetailsPage();

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void
        MapProvideDetailsPageContentToViewModel_Calls_HelpProvideDetailsPageMapper_MapProvideDetailsPageContentToViewModel()
    {
        // Arrange
        var content = new HelpProvideDetailsPage();

        // Act
        GetSut()
            .MapProvideDetailsPageContentToViewModel(content, HelpFormEnquiryReasons.GetHelp.IssueWithTheService);

        // Assert
        _mockHelpProvideDetailsPageMapper.Verify(o =>
                                                     o.MapProvideDetailsPageContentToViewModel(content,
                                                      HelpFormEnquiryReasons.GetHelp.IssueWithTheService), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_Calls_ContentService_GetHelpEmailAddressPage()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpEmailAddressPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpEmailAddressPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_ReasonIsIssueWithTheService_CallsContentServiceWithTechnicalIssueEntryId()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpEmailAddressPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpEmailAddressPage(HelpPages.TechnicalIssueEmailAddress), Times.Once);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol)]
    [DataRow(null)]
    [DataRow("")]
    public async Task GetHelpEmailAddressPage_ReasonIsNotIssueWithTheService_CallsContentServiceWithQualificationQueryEntryId(string reason)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = reason
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpEmailAddressPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpEmailAddressPage(HelpPages.QualificationQueryEmailAddress), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_ContentServiceReturnsContent_ReturnsContent()
    {
        // Arrange
        var expectedContent = new HelpEmailAddressPage
        {
            Heading = "Test Heading",
            CtaButtonText = "Continue"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpEmailAddressPage(It.IsAny<string>()))
                           .ReturnsAsync(expectedContent);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpEmailAddressPage();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedContent);
    }

    [TestMethod]
    public async Task GetHelpEmailAddressPage_ContentServiceReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpEmailAddressPage(It.IsAny<string>()))
                           .ReturnsAsync((HelpEmailAddressPage?)null);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpEmailAddressPage();

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void
        MapEmailAddressPageContentToViewModel_Calls_HelpEmailAddressPageMapper_HelpEmailAddressPageMapper_MapEmailAddressPageContentToViewModel()
    {
        // Arrange
        var content = new HelpEmailAddressPage();

        // Act
        GetSut().MapEmailAddressPageContentToViewModel(content);

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
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        var sut = GetSut();

        // Act
        _ = await sut.GetHelpConfirmationPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpConfirmationPage(It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_ReasonIsIssueWithTheService_CallsContentServiceWithTechnicalIssueEntryId()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpConfirmationPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpConfirmationPage(HelpPages.TechnicalIssueConfirmation), Times.Once);
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol)]
    [DataRow(null)]
    [DataRow("")]
    public async Task GetHelpConfirmationPage_ReasonIsNotIssueWithTheService_CallsContentServiceWithQualificationQueryEntryId(string reason)
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = reason
        });

        var sut = GetSut();

        // Act
        _ = await sut.GetHelpConfirmationPage();

        // Assert
        _mockContentService.Verify(o => o.GetHelpConfirmationPage(HelpPages.QualificationQueryConfirmation), Times.Once);
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_ContentServiceReturnsContent_ReturnsContent()
    {
        // Arrange
        var expectedContent = new HelpConfirmationPage
        {
            SuccessMessage = "Test Success Message"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpConfirmationPage(It.IsAny<string>()))
                           .ReturnsAsync(expectedContent);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpConfirmationPage();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expectedContent);
    }

    [TestMethod]
    public async Task GetHelpConfirmationPage_ContentServiceReturnsNull_ReturnsNull()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockContentService.Setup(o => o.GetHelpConfirmationPage(It.IsAny<string>()))
                           .ReturnsAsync((HelpConfirmationPage?)null);

        var sut = GetSut();

        // Act
        var result = await sut.GetHelpConfirmationPage();

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void
        MapConfirmationPageContentToViewModelAsync_Calls_HelpConfirmationPageMapper_MapConfirmationPageContentToViewModelAsync()
    {
        // Arrange
        var content = new HelpConfirmationPage();

        // Act
        GetSut().MapConfirmationPageContentToViewModelAsync(content);

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
                               _mockContentService.Object,
                               _mockUserJourneyCookieService.Object,
                               _mockNotificationService.Object,
                               _mockDateQuestionModelValidator.Object,
                               _mockHelpRadioQuestionHelpPageMapper.Object,
                               _mockHelpQualificationDetailsPageMapper.Object,
                               _mockHelpProvideDetailsPageMapper.Object,
                               _mockHelpEmailAddressPageMapper.Object,
                               _mockHelpConfirmationPageMapper.Object,
                               _mockStaticPageMapper.Object
                              );
    }
}
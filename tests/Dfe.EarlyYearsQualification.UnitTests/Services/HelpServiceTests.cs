using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
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
    private Mock<IPlaceholderUpdater> _mockPlaceholderUpdater = new();

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
        var content = new HelpQualificationDetailsPage
        {
            BeforeSeptember2014Option = new() { Value = "Before1September2014" },
            AfterSeptember2014Option = new() { StartedQuestion = new(), Value = "OnOrAfter1September2014" }
        };

        var viewModel = new QualificationDetailsPageViewModel
        {
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Value = "OnOrAfter1September2014",
                Question = new DateQuestionModel()
            },
            Before2014Option = new OptionModel { Value = "Before1September2014" }
        };

        var enquiry = new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
            QualificationStartDate = "10/2015",
            QualificationAwardedDate = "5/2017"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(enquiry);

        // Act
        GetSut().SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel, content);

        // Assert
        viewModel.Should().NotBeNull();
        viewModel.QualificationName.Should().Be(enquiry.QualificationName);
        viewModel.AwardingOrganisation.Should().Be(enquiry.AwardingOrganisation);
        viewModel.RadioButtonWithDateInputModel.Question.SelectedMonth.Should().Be(10);
        viewModel.RadioButtonWithDateInputModel.Question.SelectedYear.Should().Be(2015);
        viewModel.AwardedDate.SelectedMonth.Should().Be(5);
        viewModel.AwardedDate.SelectedYear.Should().Be(2017);
        viewModel.Option.Should().Be(content.AfterSeptember2014Option.Value);
    }

    [TestMethod]
    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie_Before2014_SetsCorrectOption()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
        {
            BeforeSeptember2014Option = new() { Value = "Before1September2014" },
            AfterSeptember2014Option = new() { StartedQuestion = new() }
        };

        var viewModel = new QualificationDetailsPageViewModel
        {
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Value = "OnOrAfter1September2014",
                Question = new DateQuestionModel()
            },
            Before2014Option = new OptionModel { Value = "Before1September2014" }
        };

        var enquiry = new HelpFormEnquiry
        {
            QualificationStartDate = "1/1900",
            QualificationAwardedDate = "6/2010",
            QualificationName = "Old Qualification",
            AwardingOrganisation = "Old Org"
        };

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(enquiry);

        // Act
        GetSut().SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel, content);

        // Assert
        viewModel.Option.Should().Be(content.BeforeSeptember2014Option.Value);
        viewModel.QualificationName.Should().Be("Old Qualification");
        viewModel.AwardingOrganisation.Should().Be("Old Org");
        viewModel.AwardedDate.SelectedMonth.Should().Be(6);
        viewModel.AwardedDate.SelectedYear.Should().Be(2010);
    }

    [TestMethod]
    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie_NoEnquiry_DoesNotThrow()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage();
        var viewModel = new QualificationDetailsPageViewModel();

        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var act = () => GetSut().SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel, content);

        // Assert
        act.Should().NotThrow();
    }

    [TestMethod]
    public void MapHelpQualificationDetailsPageContentToViewModel_Calls_Mapper()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage();
        var viewModel = new QualificationDetailsPageViewModel();

        // Act
        GetSut().MapHelpQualificationDetailsPageContentToViewModel(viewModel, content);

        // Assert
        _mockHelpQualificationDetailsPageMapper.Verify(o =>
                                                           o.MapQualificationDetailsContentToViewModel(viewModel, content),
                                                       Times.Once);
    }

    [TestMethod]
    public void SetHelpQualificationDetailsInCookie_Before2014_Updates_EnquiryValues()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var viewModel = new QualificationDetailsPageViewModel
        {
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
            Option = "Before1September2014",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel()
            },
            AwardedDate = new DateQuestionModel
            {
                SelectedMonth = 8,
                SelectedYear = 2004
            }
        };

        // Act
        GetSut().SetHelpQualificationDetailsInCookie(viewModel);

        // Assert
        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.Is<HelpFormEnquiry>(e =>
            e.QualificationStartDate == "1/1900" &&
            e.QualificationAwardedDate == "8/2004" &&
            e.QualificationName == "Test Qualification Name" &&
            e.AwardingOrganisation == "Test Awarding Organisation"
        )), Times.Once);
    }

    [TestMethod]
    public void SetHelpQualificationDetailsInCookie_After2014_Updates_EnquiryValues()
    {
        // Arrange
        _mockUserJourneyCookieService.Setup(o => o.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var viewModel = new QualificationDetailsPageViewModel
        {
            AwardingOrganisation = "Test Awarding Organisation",
            QualificationName = "Test Qualification Name",
            Option = "OnOrAfter1September2014",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel
                {
                    SelectedMonth = 9,
                    SelectedYear = 2020
                }
            },
            AwardedDate = new DateQuestionModel
            {
                SelectedMonth = 12,
                SelectedYear = 2022
            }
        };

        // Act
        GetSut().SetHelpQualificationDetailsInCookie(viewModel);

        // Assert
        _mockUserJourneyCookieService.Verify(o => o.SetHelpFormEnquiry(It.Is<HelpFormEnquiry>(e =>
            e.QualificationStartDate == "9/2020" &&
            e.QualificationAwardedDate == "12/2022" &&
            e.QualificationName == "Test Qualification Name" &&
            e.AwardingOrganisation == "Test Awarding Organisation"
        )), Times.Once);
    }

    [TestMethod]
    public void ValidateDates_Calls_QuestionModelValidator_IsValid()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel();
        var content = new HelpQualificationDetailsPage();

        // Act
        GetSut().ValidateDates(model, content);

        // Assert
        _mockDateQuestionModelValidator.Verify(o => o.IsValid(model, content), Times.Once);
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

    [TestMethod]
    public void DateIsValid_Calls_QuestionModelValidator_IsValid()
    {
        // Arrange
        var model = new DateQuestionModel();
        var content = new DateQuestion();

        // Act
        GetSut().DateIsValid(model, content);

        // Assert
        _mockDateQuestionModelValidator.Verify(o => o.IsValid(model, content), Times.Once);
    }

    [TestMethod]
    public void DateIsValid_ReturnsValidationResult()
    {
        // Arrange
        var model = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2020 };
        var content = new DateQuestion();
        var expectedResult = new DateValidationResult { MonthValid = true, YearValid = true };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model, content))
                                      .Returns(expectedResult);

        // Act
        var result = GetSut().DateIsValid(model, content);

        // Assert
        result.Should().BeSameAs(expectedResult);
    }

    [TestMethod]
    public void MapDateModel_ReplacesPlaceholders_AndMapsCorrectly()
    {
        // Arrange
        var model = new DateQuestionModel { SelectedMonth = 3, SelectedYear = 2021 };
        var question = new DateQuestion
        {
            QuestionHint = "Test hint",
            ErrorMessage = "Test error message with [placeholder]"
        };
        var validationResult = new DateValidationResult
        {
            MonthValid = false,
            YearValid = true,
            ErrorMessages = new List<string> { "Month is invalid" },
            BannerErrorMessages = new List<BannerError>
            {
                new BannerError("Banner error with [placeholder]", FieldId.Month)
            }
        };

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s.Replace("[placeholder]", "replaced"));

        // Act
        var result = GetSut().MapDateModel(model, question, validationResult, "TestObject");

        // Assert
        result.Should().NotBeNull();
        result.SelectedMonth.Should().Be(3);
        result.SelectedYear.Should().Be(2021);
        _mockPlaceholderUpdater.Verify(o => o.Replace(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [TestMethod]
    public void MapDateModel_UpdatesErrorSummaryLinksWithObjectName()
    {
        // Arrange
        var model = new DateQuestionModel();
        var question = new DateQuestion();
        var validationResult = new DateValidationResult
        {
            MonthValid = false,
            YearValid = false,
            ErrorMessages = new List<string> { "Invalid date" },
            BannerErrorMessages = new List<BannerError>
            {
                new BannerError("Month error", FieldId.Month),
                new BannerError("Year error", FieldId.Year)
            }
        };

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        var result = GetSut().MapDateModel(model, question, validationResult, "AwardedDate");

        // Assert
        result.ErrorSummaryLinks.Should().Contain(e => e.ElementLinkId == "AwardedDate.SelectedMonth");
        result.ErrorSummaryLinks.Should().Contain(e => e.ElementLinkId == "AwardedDate.SelectedYear");
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_AllFieldsValid_NoErrors()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            QualificationName = "Valid Name",
            AwardingOrganisation = "Valid Org",
            Option = "Before1September2014",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            AwardedDate = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2020 }
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" },
            AfterSeptember2014Option = new RadioButtonAndDateInput
            {
                StartedQuestion = new DateQuestion()
            }
        };

        var modelState = new ModelStateDictionary();

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.Errors.Should().BeEmpty();
        model.HasQualificationNameError.Should().BeFalse();
        model.HasOptionError.Should().BeFalse();
        model.HasAwardingOrganisationError.Should().BeFalse();
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_QualificationNameInvalid_AddsError()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            QualificationNameErrorMessage = "Qualification name is required",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            Option = "Before1September2014",
            AwardedDate = new DateQuestionModel()
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" }
        };

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("QualificationName", "Required");

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.HasQualificationNameError.Should().BeTrue();
        model.Errors.Should().Contain(e => e.ElementLinkId == "QualificationName");
        model.Errors.Should().Contain(e => e.ErrorBannerLinkText == "Qualification name is required");
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_OptionInvalid_AddsError()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            MissingStartedDateOptionErrorMessage = "Please select an option",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            AwardedDate = new DateQuestionModel()
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" }
        };

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Option", "Required");

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.HasOptionError.Should().BeTrue();
        model.Errors.Should().Contain(e => e.ElementLinkId == "Before1September2014");
        model.Errors.Should().Contain(e => e.ErrorBannerLinkText == "Please select an option");
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_AwardingOrganisationInvalid_AddsError()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            AwardingOrganisationErrorMessage = "Awarding organisation is required",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            Option = "Before1September2014",
            AwardedDate = new DateQuestionModel()
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" }
        };

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("AwardingOrganisation", "Required");

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.HasAwardingOrganisationError.Should().BeTrue();
        model.Errors.Should().Contain(e => e.ElementLinkId == "AwardingOrganisation");
        model.Errors.Should().Contain(e => e.ErrorBannerLinkText == "Awarding organisation is required");
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_After2014WithInvalidDates_AddsDateErrors()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Option = "OnOrAfter1September2014",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel { SelectedMonth = 0, SelectedYear = 2020 }
            },
            AwardedDate = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2020 }
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion { ErrorMessage = "Invalid awarded date" },
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" },
            AfterSeptember2014Option = new RadioButtonAndDateInput
            {
                StartedQuestion = new DateQuestion { ErrorMessage = "Invalid started date" }
            }
        };

        var modelState = new ModelStateDictionary();

        var startedValidationResult = new DateValidationResult
        {
            MonthValid = false,
            YearValid = true,
            ErrorMessages = new List<string> { "Invalid month" },
            BannerErrorMessages = new List<BannerError>
            {
                new BannerError("Month must be between 1 and 12", FieldId.Month)
            }
        };

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        var datesValidationResult = new DatesValidationResult
        {
            StartedValidationResult = startedValidationResult,
            AwardedValidationResult = awardedValidationResult
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model, content))
                                      .Returns(datesValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.Errors.Should().NotBeEmpty();
        model.Errors.Should().Contain(e => e.ElementLinkId.Contains("RadioButtonWithDateInputModel.Question"));
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_Before2014_RemovesStartedDateFromModelState()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            Option = "Before1September2014",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            RadioButtonWithDateInputModel = new RadioButtonAndDateInputModel
            {
                Question = new DateQuestionModel()
            },
            AwardedDate = new DateQuestionModel { SelectedMonth = 5, SelectedYear = 2020 }
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" }
        };

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("RadioButtonWithDateInputModel.Question.SelectedMonth", "Some error");
        modelState.AddModelError("RadioButtonWithDateInputModel.Question.SelectedYear", "Some error");

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = true,
            YearValid = true,
            ErrorMessages = new List<string>(),
            BannerErrorMessages = new List<BannerError>()
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        modelState.Should().NotContainKey("RadioButtonWithDateInputModel.Question.SelectedMonth");
        modelState.Should().NotContainKey("RadioButtonWithDateInputModel.Question.SelectedYear");
    }

    [TestMethod]
    public void AddQualificationDetailsValidationErrors_MultipleErrors_AddsAllErrors()
    {
        // Arrange
        var model = new QualificationDetailsPageViewModel
        {
            QualificationNameErrorMessage = "Name required",
            AwardingOrganisationErrorMessage = "Organisation required",
            MissingStartedDateOptionErrorMessage = "Option required",
            Before2014Option = new OptionModel { Value = "Before1September2014" },
            AwardedDate = new DateQuestionModel()
        };

        var content = new HelpQualificationDetailsPage
        {
            AwardedDateQuestion = new DateQuestion(),
            BeforeSeptember2014Option = new Option { Value = "Before1September2014" }
        };

        var modelState = new ModelStateDictionary();
        modelState.AddModelError("QualificationName", "Required");
        modelState.AddModelError("AwardingOrganisation", "Required");
        modelState.AddModelError("Option", "Required");

        var awardedValidationResult = new DateValidationResult
        {
            MonthValid = false,
            YearValid = false,
            ErrorMessages = new List<string> { "Invalid date" },
            BannerErrorMessages = new List<BannerError>
            {
                new BannerError("Date error", FieldId.Month)
            }
        };

        _mockDateQuestionModelValidator.Setup(o => o.IsValid(model.AwardedDate, content.AwardedDateQuestion))
                                      .Returns(awardedValidationResult);

        _mockPlaceholderUpdater.Setup(o => o.Replace(It.IsAny<string>()))
                               .Returns<string>(s => s);

        // Act
        GetSut().AddQualificationDetailsValidationErrors(model, content, modelState);

        // Assert
        model.Errors.Count.Should().BeGreaterThan(0);
        model.HasQualificationNameError.Should().BeTrue();
        model.HasAwardingOrganisationError.Should().BeTrue();
        model.HasOptionError.Should().BeTrue();
        model.Errors.Should().Contain(e => e.ElementLinkId == "QualificationName");
        model.Errors.Should().Contain(e => e.ElementLinkId == "AwardingOrganisation");
        model.Errors.Should().Contain(e => e.ElementLinkId == "Before1September2014");
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
                               _mockStaticPageMapper.Object,
                               _mockPlaceholderUpdater.Object
                              );
    }
}
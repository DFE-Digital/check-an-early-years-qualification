using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HelpControllerTests
{
    Mock<ILogger<HelpController>> mockLogger = new Mock<ILogger<HelpController>>();
    Mock<IContentService> mockContentService = new Mock<IContentService>();
    Mock<INotificationService> mockNotificationService = new Mock<INotificationService>();
    Mock<IDateQuestionModelValidator> mockDateQuestionValidator = new Mock<IDateQuestionModelValidator>();
    Mock<IUserJourneyCookieService> mockUserJourneyService = new Mock<IUserJourneyCookieService>();
    Mock<IHelpGetHelpPageMapper> mockGetPageMapper = new Mock<IHelpGetHelpPageMapper>();
    Mock<IHelpQualificationDetailsPageMapper> mockQualificationDetailsPageMapper = new Mock<IHelpQualificationDetailsPageMapper>();
    Mock<IHelpProvideDetailsPageMapper> mockProvideDetailsPageMapper = new Mock<IHelpProvideDetailsPageMapper>();
    Mock<IHelpEmailAddressPageMapper> mockEmailAddressMapper = new Mock<IHelpEmailAddressPageMapper>();
    Mock<IHelpConfirmationPageMapper> mockConfirmationMapper = new Mock<IHelpConfirmationPageMapper>();

    private HelpController GetSut()
    {
        return new HelpController(mockLogger.Object,
                                mockContentService.Object,
                                mockUserJourneyService.Object,
                                mockNotificationService.Object,
                                mockDateQuestionValidator.Object,
                                mockGetPageMapper.Object,
                                mockQualificationDetailsPageMapper.Object,
                                mockProvideDetailsPageMapper.Object,
                                mockEmailAddressMapper.Object,
                                mockConfirmationMapper.Object
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
        mockGetPageMapper.Setup(x => x.MapGetHelpPageContentToViewModelAsync(content)).Returns(Task.FromResult(
            new GetHelpPageViewModel()
            {
                Heading = content.Heading,
                PostHeadingContent = "Test html body"
            }
        ));

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
                   Label = HelpFormEnquiryReasons.QuestionAboutAQualification
               },
               new EnquiryOption
               {
                   Value = "IssueWithTheService",
                   Label = HelpFormEnquiryReasons.IssueWithTheService
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
                    Label = HelpFormEnquiryReasons.QuestionAboutAQualification
                },
                new EnquiryOptionModel()
                {
                    Value = "IssueWithTheService",
                    Label = HelpFormEnquiryReasons.IssueWithTheService
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
        var content = new GetHelpPage();

        mockContentService.Setup(x => x.GetGetHelpPage()).ReturnsAsync(content);

        mockGetPageMapper.Setup(x => x.MapGetHelpPageContentToViewModelAsync(content)).Returns(Task.FromResult(new GetHelpPageViewModel()));

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
            ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var startedAt = (1, 2000);
        var awardedAt = (6, 2002);

        mockUserJourneyService.Setup(x => x.GetAwardingOrganisation()).Returns("Awarding organisation");
        mockUserJourneyService.Setup(x => x.GetSelectedQualificationName()).Returns("Qualification name");

        mockUserJourneyService.Setup(x => x.GetWhenWasQualificationStarted()).Returns(startedAt);
        mockUserJourneyService.Setup(x => x.GetWhenWasQualificationAwarded()).Returns(awardedAt);

        var controller = GetSut();

        var viewModel = new QualificationDetailsPageViewModel()
        {
            Heading = content.Heading,
            BackButton = new NavigationLinkModel()
            {
                DisplayText = content.BackButton.DisplayText,
                Href = content.BackButton.Href
            },
            QualificationName = "Qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = startedAt.Item1,
                    SelectedYear = startedAt.Item2
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = awardedAt.Item1,
                    SelectedYear = awardedAt.Item2
                }
            },
            AwardingOrganisation = "Some organisation where the qualification is from",
        };


        var e = new DatesValidationResult()
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
        };
        mockQualificationDetailsPageMapper.Setup(x => x.MapDateModel(viewModel.QuestionModel.StartedQuestion, content.StartDateQuestion, e.StartedValidationResult)).Returns(viewModel.QuestionModel.StartedQuestion);
        mockQualificationDetailsPageMapper.Setup(x => x.MapDateModel(viewModel.QuestionModel.AwardedQuestion, content.AwardedDateQuestion, e.AwardedValidationResult)).Returns(viewModel.QuestionModel.AwardedQuestion);

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(It.IsAny<QualificationDetailsPageViewModel>(), content, null, It.IsAny<ModelStateDictionary>())).Returns(viewModel);

        // Act
        var result = await controller.QualificationDetails();

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
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
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

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.QuestionAboutAQualification);

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
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
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

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.QuestionAboutAQualification);

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
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };
        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            }
        );

        var vm = new QualificationDetailsPageViewModel()
        {
            QualificationName = null,
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2001
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2002
                }
            },
            AwardingOrganisation = "awarding org",
        };

        var validationResult = new DatesValidationResult()
        {
            StartedValidationResult = new(),
            AwardedValidationResult = new()
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>())).Returns(validationResult);

        var controller = GetSut();

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(vm, content, validationResult, controller.ModelState)).Returns(vm);

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
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };
        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            }
        );

        var vm = new QualificationDetailsPageViewModel()
        {
            QualificationName = "qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2001
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2002
                }
            },
            AwardingOrganisation = null,
        };

        var validationResult = new DatesValidationResult()
        {
            StartedValidationResult = new(),
            AwardedValidationResult = new()
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>())).Returns(validationResult);

        var controller = GetSut();

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(vm, content, validationResult, controller.ModelState)).Returns(vm);

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
        var content = new HelpQualificationDetailsPage
        {
            Heading = "Heading",
        };
        mockContentService.Setup(x => x.GetHelpQualificationDetailsPage()).ReturnsAsync(content);

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry()
            {
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            }
        );

        var vm = new QualificationDetailsPageViewModel()
        {
            QualificationName = "qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2001
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = null,
                    SelectedYear = 2002
                },
            },
            AwardingOrganisation = "awarding org",
        };

        var validationResult = new DatesValidationResult()
        {
            StartedValidationResult = new(),
            AwardedValidationResult = new() { MonthValid = false }
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>())).Returns(validationResult);

        var controller = GetSut();

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(vm, content, validationResult, controller.ModelState)).Returns(vm);

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
    public async Task Post_QualificationDetails_MissingRequiredFieldAwardedYear_ReturnsProvideDetailsPageViewModel()
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
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            }
        );

        var vm = new QualificationDetailsPageViewModel()
        {
            QualificationName = "qualification name",
            QuestionModel = new DatesQuestionModel()
            {
                StartedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = 2001
                },
                AwardedQuestion = new()
                {
                    SelectedMonth = 1,
                    SelectedYear = null
                }
            },
            AwardingOrganisation = "awarding org",
        };

        var validationResult = new DatesValidationResult()
        {
            StartedValidationResult = new(),
            AwardedValidationResult = new() { YearValid = false }
        };

        mockDateQuestionValidator.Setup(x => x.IsValid(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>())).Returns(validationResult);

        var controller = GetSut();

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(vm, content, validationResult, controller.ModelState)).Returns(vm);

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
    [DataRow(HelpFormEnquiryReasons.QuestionAboutAQualification, "QualificationDetails")]
    [DataRow(HelpFormEnquiryReasons.IssueWithTheService, "GetHelp")]
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

        var enquiry = new HelpFormEnquiry()
        {
            ReasonForEnquiring = selectedOption,
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

        var viewModel = new ProvideDetailsPageViewModel();

        if (pageToRedirectTo == "GetHelp")
        {
            viewModel = new ProvideDetailsPageViewModel()
            {
                Heading = content.Heading,
                BackButton = new()
                {
                    DisplayText = content.BackButtonToGetHelpPage.DisplayText,
                    Href = content.BackButtonToGetHelpPage.Href
                }
            };
        }

        if (pageToRedirectTo == "QualificationDetails")
        {
            viewModel = new ProvideDetailsPageViewModel()
            {
                Heading = content.Heading,
                BackButton = new()
                {
                    DisplayText = content.BackButtonToQualificationDetailsPage.DisplayText,
                    Href = content.BackButtonToQualificationDetailsPage.Href
                }
            };
        }

        mockProvideDetailsPageMapper.Setup(x => x.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring)).Returns(viewModel);

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
            ReasonForEnquiring = HelpFormEnquiryReasons.IssueWithTheService,
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

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.IssueWithTheService);
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
        var enquiry = new HelpFormEnquiry()
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.IssueWithTheService,
        };

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

        var content = new HelpProvideDetailsPage();

        mockContentService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(content);

        mockProvideDetailsPageMapper.Setup(x => x.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring)).Returns(new ProvideDetailsPageViewModel());

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
        mockEmailAddressMapper.Setup(x => x.MapEmailAddressPageContentToViewModel(content)).Returns(
            new EmailAddressPageViewModel()
            {
                Heading = content.Heading,
            }
        );

        var helpForm = new HelpFormEnquiry()
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
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
            ReasonForEnquiring = HelpFormEnquiryReasons.IssueWithTheService,
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

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.IssueWithTheService);
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
        var content = new HelpEmailAddressPage();

        mockContentService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(content);
        mockEmailAddressMapper.Setup(x => x.MapEmailAddressPageContentToViewModel(content)).Returns(new EmailAddressPageViewModel());

        mockUserJourneyService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(EmailAddressPageViewModel.EmailAddress), "Invalid");

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
        mockConfirmationMapper.Setup(x => x.MapConfirmationPageContentToViewModelAsync(content)).Returns(Task.FromResult(
            new ConfirmationPageViewModel()
            {
                SuccessMessage = content.SuccessMessage,
            }
        ));

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
                ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification,
            }
        );

        mockQualificationDetailsPageMapper.Setup(x => x.MapQualificationDetailsContentToViewModel(new(), content, null , new())).Returns(new QualificationDetailsPageViewModel());

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
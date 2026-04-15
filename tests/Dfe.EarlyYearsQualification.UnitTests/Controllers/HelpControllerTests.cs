using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class HelpControllerTests
{
    private readonly Mock<ILogger<HelpController>> _mockLogger = new Mock<ILogger<HelpController>>();
    private readonly Mock<IHelpService> _mockHelpService = new Mock<IHelpService>();

    // Get help tests

    [TestMethod]
    public async Task GetHelp_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().GetHelp();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Get help page' content could not be found");
    }

    [TestMethod]
    public async Task GetHelp_ContentServiceReturnsGetHelpPage_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var content = new RadioQuestionHelpPage { Heading = "Heading" };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);
        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content)).Returns(Task.FromResult(
             new RadioQuestionHelpPageViewModel
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

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.PostHeadingContent.Should().Be("Test html body");
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification)]
    [DataRow(HelpFormEnquiryReasons.GetHelp.IssueWithTheService)]
    public async Task GetHelp_ContentServiceReturnsGetHelpPage_EnquiryIsPrepopulated(string selectedOption)
    {
        // Arrange
        var content = new RadioQuestionHelpPage
                      {
                          Heading = "Heading",
                          Options = new List<Option>
                                           {
                                               new Option
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript),
                                                   Label = HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript
                                               },
                                               new Option
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs),
                                                   Label = HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs
                                               },
                                               new Option
                                               {
                                                   Value = nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol),
                                                   Label = HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol
                                               },
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

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        var enquiry = new HelpFormEnquiry
                      {
                          ReasonForEnquiring = selectedOption,
                      };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

        _mockHelpService.Setup(x => x.GetWhyAreYouContactingUsSelectedOption())
                        .Returns(content.Options.First(x => x.Label == selectedOption).Value);

        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content)).Returns(Task.FromResult(
             new RadioQuestionHelpPageViewModel
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

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.PostHeadingContent.Should().Be("Test html body");

        model.SelectedOption.Should().Be(content.Options.First(x => x.Label == selectedOption).Value);
    }

    [TestMethod]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript), nameof(HelpController.INeedACopyOfTheQualificationCertificateOrTranscript))]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs), nameof(HelpController.IDoNotKnowWhatLevelTheQualificationIs))]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol), nameof(HelpController.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol))]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification), nameof(HelpController.ProceedWithQualificationQuery))]
    [DataRow(nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService), nameof(HelpController.ProvideDetails))]
    public async Task Post_GetHelp_ValidModelStateRedirectsToExpectedPage(
        string selectedOption, string pageToRedirectTo)
    {
        // Arrange
        var content = new RadioQuestionHelpPage
                      {
                          Options = new List<Option>
                                            {
                                                new Option
                                                {
                                                    Value = nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript),
                                                    Label = HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript
                                                },
                                                new Option
                                                {
                                                    Value = nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs),
                                                    Label = HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs
                                                },
                                                new Option
                                                {
                                                    Value = nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol),
                                                    Label = HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol
                                                },
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

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.SelectedOptionIsValid(content.Options, It.IsAny<string>()))
                        .Returns(() => true);

        var submittedViewModel = new RadioQuestionHelpPageViewModel
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

        _mockHelpService.Verify(x => x.SetHelpFormEnquiryReason(submittedViewModel.SelectedOption), Times.Once);
    }

    [TestMethod]
    public async Task Post_GetHelp_InvalidEnquiryOption_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var submission = new RadioQuestionHelpPageViewModel
                         {
                             SelectedOption = "invalid enquiry option",
                             Options = new List<OptionModel>
                                              {
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript),
                                                      Label = HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript
                                                  },
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs),
                                                      Label = HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs
                                                  },
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol),
                                                      Label = HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol
                                                  },
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService),
                                                      Label = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
                                                  },
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification),
                                                      Label = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification
                                                  },
                                                  new OptionModel
                                                  {
                                                      Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService),
                                                      Label = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
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
    public async Task Post_GetHelp_InvalidModelState_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var content = new RadioQuestionHelpPage
                      {
                          NoEnquiryOptionSelectedErrorMessage = "Select reason for enquiring",
                          ErrorBannerHeading = "There is a problem"
                      };

        var viewModel = new RadioQuestionHelpPageViewModel
                        {
                            ErrorBannerHeading = content.ErrorBannerHeading,
                            NoEnquiryOptionSelectedErrorMessage = content.NoEnquiryOptionSelectedErrorMessage,
                            Options = new()
                                             {
                                                 new()
                                                 {
                                                     Label = "option1",
                                                     Value = "option1"
                                                 }
                                             }
                        };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content))
                        .Returns(Task.FromResult(viewModel));

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(RadioQuestionHelpPageViewModel.SelectedOption), "Invalid");

        // Act
        var result = await controller.GetHelp(viewModel);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.HasNoEnquiryOptionSelectedError.Should().BeTrue();

        model.ErrorSummaryModel.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ElementLinkId.Should()
             .Be(viewModel.Options.First().Label);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ErrorBannerLinkText.Should()
             .Be(content.NoEnquiryOptionSelectedErrorMessage);
    }

    // static page tests

    [TestMethod]
    public async Task Get_INeedACopyOfTheQualificationCertificateOrTranscript_ReturnsStaticPageModel()
    {
        // Arrange
        SetupStaticPage();

        // Act
        var result = await GetSut().INeedACopyOfTheQualificationCertificateOrTranscript();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as StaticPageModel;
        model.Should().NotBeNull();
        model.Heading.Should().Be("Static page heading");
    }

    [TestMethod]
    public async Task Get_IDoNotKnowWhatLevelTheQualificationIs_ReturnsStaticPageModel()
    {
        // Arrange
        SetupStaticPage();

        // Act
        var result = await GetSut().IDoNotKnowWhatLevelTheQualificationIs();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as StaticPageModel;
        model.Should().NotBeNull();
        model.Heading.Should().Be("Static page heading");
    }

    [TestMethod]
    public async Task Get_IWantToCheckWhetherACourseIsApprovedBeforeIEnrol_ReturnsStaticPageModel()
    {
        // Arrange
        SetupStaticPage();

        // Act
        var result = await GetSut().IWantToCheckWhetherACourseIsApprovedBeforeIEnrol();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as StaticPageModel;
        model.Should().NotBeNull();
        model.Heading.Should().Be("Static page heading");
    }

    [TestMethod]
    public async Task Get_IWantToCheckWhetherACourseIsApprovedBeforeIEnrol_NoContentReturned_RedirecToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetStaticPage(It.IsAny<string>())).ReturnsAsync(() => null);

        // Act
        var result = await GetSut().IWantToCheckWhetherACourseIsApprovedBeforeIEnrol();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("No content for the static page");
    }

    // proceed with qualification query tests
    [TestMethod]
    public async Task GetRadioQuestionHelpPageAsync_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().ProceedWithQualificationQuery();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
        _mockLogger.VerifyError("'Proceed with qualification query page' content could not be found");
    }

    [TestMethod]
    public async Task GetRadioQuestionHelpPageAsync_ContentService_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var content = new RadioQuestionHelpPage { Heading = "Heading" };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);
        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content)).Returns(Task.FromResult(
             new RadioQuestionHelpPageViewModel
             {
                 Heading = content.Heading,
                 PostHeadingContent = "Test html body"
             }
            ));

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry
            {
                ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification
            }
        );

        // Act
        var result = await GetSut().ProceedWithQualificationQuery();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.PostHeadingContent.Should().Be("Test html body");
    }

    [TestMethod]
    public async Task Get_ProceedWithQualificationQuery_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());
        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(new RadioQuestionHelpPage());
        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(It.IsAny<RadioQuestionHelpPage>())).ReturnsAsync(It.IsAny<RadioQuestionHelpPageViewModel>());

        var submittedViewModel = new RadioQuestionHelpPageViewModel
        {
            SelectedOption = HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam,
        };

        // Act
        var result = await GetSut().ProceedWithQualificationQuery();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    [DataRow(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService)]
    [DataRow(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam)]
    public async Task ProceedWithQualificationQuery_ContentServiceReturnsRadioQuestionHelpPage_EnquiryIsPrepopulated(string selectedOption)
    {
        // Arrange
        var content = new RadioQuestionHelpPage
        {
            Heading = "Heading",
            Options = 
            new List<Option>
            {
                new Option
                {
                    Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService),
                    Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService
                },
                new Option
                {
                    Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam),
                    Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam
                },
            }
        };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
            new HelpFormEnquiry
            {
                ReasonForEnquiring = selectedOption
            }
        );

        _mockHelpService.Setup(x => x.GetWhatDoYouWantToDoNextSelectedOption())
                        .Returns(content.Options.First(x => x.Label == selectedOption).Value);

        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content)).Returns(
            Task.FromResult(
                new RadioQuestionHelpPageViewModel
                {
                    Heading = content.Heading,
                    PostHeadingContent = "Test html body"
                }
            )
        );

        // Act
        var result = await GetSut().ProceedWithQualificationQuery();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.Heading.Should().Be(content.Heading);
        model.PostHeadingContent.Should().Be("Test html body");

        model.SelectedOption.Should().Be(content.Options.First(x => x.Label == selectedOption).Value);
    }

    [TestMethod]
    public async Task Post_ProceedWithQualificationQuery_CheckTheQualificationUsingTheService_ValidModelStateRedirectsToExpectedPage()
    {
        // Arrange
        var content = new RadioQuestionHelpPage
        {
            Options =
                new List<Option>
                {
                    new Option
                    {
                        Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService),
                        Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService
                    },
                    new Option
                    {
                        Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam),
                        Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam
                    },
                }
        };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.SelectedOptionIsValid(content.Options, It.IsAny<string>()))
                        .Returns(() => true);

        var submittedViewModel = new RadioQuestionHelpPageViewModel
        {
            SelectedOption = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService),
        };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(It.IsAny<HelpFormEnquiry>());

        // Act
        var result = await GetSut().ProceedWithQualificationQuery(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task Post_ProceedWithQualificationQuery_ContactTheEarlyYearsQualificationTeam_ValidModelStateRedirectsToExpectedPage()
    {
        // Arrange
        var content = new RadioQuestionHelpPage
        {
            Options = 
                new List<Option>
                {
                    new Option
                    {
                        Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService),
                        Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService
                    },
                    new Option
                    {
                        Value = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam),
                        Label = HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam
                    },
                }
        };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.SelectedOptionIsValid(content.Options, It.IsAny<string>()))
                        .Returns(() => true);

        var submittedViewModel = new RadioQuestionHelpPageViewModel
        {
            SelectedOption = nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam),
        };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().ProceedWithQualificationQuery(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("QualificationDetails");


        _mockHelpService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_ProceedWithQualificationQuery_InvalidEnquiryOption_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var submission = new RadioQuestionHelpPageViewModel
        {
            SelectedOption = "invalid enquiry option",
            Options = 
                new List<OptionModel>
                {
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript),
                        Label = HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript
                    },
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs),
                        Label = HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs
                    },
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol),
                        Label = HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol
                    },
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService),
                        Label = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
                    },
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification),
                        Label = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification
                    },
                    new OptionModel
                    {
                        Value = nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService),
                        Label = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
                    }
                }
        };

        // Act
        var result = await GetSut().ProceedWithQualificationQuery(submission);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }

    [TestMethod]
    public async Task Post_ProceedWithQualificationQuery_InvalidModelState_ReturnsRadioQuestionHelpPageViewModel()
    {
        // Arrange
        var content = new RadioQuestionHelpPage
        {
            NoEnquiryOptionSelectedErrorMessage = "Select reason for enquiring",
            ErrorBannerHeading = "There is a problem"
        };

        var viewModel = new RadioQuestionHelpPageViewModel
        {
            ErrorBannerHeading = content.ErrorBannerHeading,
            NoEnquiryOptionSelectedErrorMessage = content.NoEnquiryOptionSelectedErrorMessage,
            Options = 
                new()
                {
                    new()
                    {
                        Label = "option1",
                        Value = "option1"
                    }
                }
            };

        _mockHelpService.Setup(x => x.GetRadioQuestionHelpPageAsync(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.MapRadioQuestionHelpPageContentToViewModelAsync(content))
                        .Returns(Task.FromResult(viewModel));

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(RadioQuestionHelpPageViewModel.SelectedOption), "Invalid");

        // Act
        var result = await controller.ProceedWithQualificationQuery(viewModel);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as RadioQuestionHelpPageViewModel;
        model.Should().NotBeNull();

        model.HasNoEnquiryOptionSelectedError.Should().BeTrue();

        model.ErrorSummaryModel.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ElementLinkId.Should()
             .Be(viewModel.Options.First().Label);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ErrorBannerLinkText.Should()
             .Be(content.NoEnquiryOptionSelectedErrorMessage);
    }

    // Help qualification details tests

    [TestMethod]
    public async Task QualificationDetails_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help qualification details page' content could not be found");
    }

    [TestMethod]
    public async Task QualificationDetails_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(() => new()).Verifiable();

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task
        QualificationDetails_ContentServiceReturnsHelpProvideDetailsPage_ReturnsProvideDetailsPageViewModel_PrepopulatedWithQualificationDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
                      {
                          Heading = "Heading",
                          BackButton = new NavigationLink
                                       {
                                           Href = "/get-help",
                                           DisplayText = "Back to get help"
                                       }
                      };

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        var enquiry = new HelpFormEnquiry
                      {
                          ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
                          AwardingOrganisation = "Awarding organisation",
                          QualificationName = "Qualification name"
                      };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

        var startedAt = (1, 2000);
        var awardedAt = (6, 2002);

        var controller = GetSut();

        var viewModel = new QualificationDetailsPageViewModel
                        {
                            Heading = content.Heading,
                            BackButton = new NavigationLinkModel
                                         {
                                             DisplayText = content.BackButton.DisplayText,
                                             Href = content.BackButton.Href
                                         },
                            QualificationName = enquiry.QualificationName,
                            QuestionModel = new DatesQuestionModel
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
                            AwardingOrganisation = enquiry.AwardingOrganisation,
                        };

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new DateValidationResult
                                                             {
                                                                 MonthValid = true,
                                                                 YearValid = true
                                                             },
                                   AwardedValidationResult = new DateValidationResult
                                                             {
                                                                 MonthValid = true,
                                                                 YearValid = true
                                                             }
                               };

        _mockHelpService.Setup(x => x.HasInvalidDates(validationResult)).Returns(false);

        _mockHelpService
            .Setup(x =>
                       x.MapHelpQualificationDetailsPageContentToViewModel(It.IsAny<QualificationDetailsPageViewModel>(),
                                                                           content, null,
                                                                           It.IsAny<ModelStateDictionary>()))
            .Returns(viewModel);

        // Act
        var result = await controller.QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();
        model.BackButton.Should().NotBeNull();
        model.QuestionModel.StartedQuestion.Should().NotBeNull();
        model.QuestionModel.AwardedQuestion.Should().NotBeNull();
        model.QualificationName.Should().Be(enquiry.QualificationName);
        model.AwardingOrganisation.Should().Be(enquiry.AwardingOrganisation);

        model.Heading.Should().Be(content.Heading);
        model.BackButton.DisplayText.Should().Be(content.BackButton.DisplayText);
        model.BackButton.Href.Should().Be(content.BackButton.Href);

        model.QuestionModel.StartedQuestion.SelectedMonth.Should().Be(startedAt.Item1);
        model.QuestionModel.StartedQuestion.SelectedYear.Should().Be(startedAt.Item2);
        model.QuestionModel.AwardedQuestion.SelectedMonth.Should().Be(awardedAt.Item1);
        model.QuestionModel.AwardedQuestion.SelectedYear.Should().Be(awardedAt.Item2);
    }

    [TestMethod]
    public async Task
        QualificationDetails_ContentServiceReturnsHelpProvideDetailsPage_ReturnsProvideDetailsPageViewModel_PrepopulatedWithPreviouslyEnteredDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage();

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        var startedAt = (1, 2000);
        var awardedAt = (6, 2002);

        var viewModel = new QualificationDetailsPageViewModel
                        {
                            Heading = content.Heading,
                            BackButton = new NavigationLinkModel
                                         {
                                             DisplayText = content.BackButton.DisplayText,
                                             Href = content.BackButton.Href
                                         },
                            QualificationName = "Qualification name",
                            QuestionModel = new DatesQuestionModel
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

        var helpForm = new HelpFormEnquiry
                       {
                           ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
                           QualificationStartDate = "1/2001",
                           QualificationAwardedDate = "2/2002",
                           QualificationName = "Some qualification name",
                           AwardingOrganisation = "Some organisation"
                       };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        _mockHelpService
            .Setup(x =>
                       x.MapHelpQualificationDetailsPageContentToViewModel(It.IsAny<QualificationDetailsPageViewModel>(),
                                                                           content, null,
                                                                           It.IsAny<ModelStateDictionary>()))
            .Returns(viewModel);

        // Act
        var result = await GetSut().QualificationDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as QualificationDetailsPageViewModel;
        model.Should().NotBeNull();
        model.QuestionModel.StartedQuestion.Should().NotBeNull();
        model.QuestionModel.AwardedQuestion.Should().NotBeNull();
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

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                        QualificationName = "Qualification name",
                                                                        QualificationStartDate = "1/2000",
                                                                        QualificationAwardedDate = "5/2003",
                                                                        AwardingOrganisation =
                                                                            "Some organisation where the qualification is from"
                                                                    }
                                                                   );

        var submittedViewModel = new QualificationDetailsPageViewModel
                                 {
                                     QualificationName = "Qualification name",
                                     QuestionModel = new DatesQuestionModel
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

        _mockHelpService.Setup(x => x.ValidateDates(submittedViewModel.QuestionModel, content)).Returns(
             new DatesValidationResult
             {
                 StartedValidationResult = new DateValidationResult
                                           {
                                               MonthValid = true,
                                               YearValid = true
                                           },
                 AwardedValidationResult = new DateValidationResult
                                           {
                                               MonthValid = true,
                                               YearValid = true
                                           }
             }
            );

        // Act
        var result = await GetSut().QualificationDetails(submittedViewModel);

        var enquiry = _mockHelpService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();
        enquiry.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification);

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

        _mockHelpService.Verify(x => x.SetHelpQualificationDetailsInCookie(It.IsAny<HelpFormEnquiry>(),
                                                                           submittedViewModel), Times.Once);
    }

    [TestMethod]
    public async Task Post_QualificationDetails_ValidModelState_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
                      {
                          Heading = "Heading",
                      };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        var submittedViewModel = new QualificationDetailsPageViewModel
                                 {
                                     QualificationName = "Qualification name",
                                     QuestionModel = new DatesQuestionModel
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

        _mockHelpService.Setup(x => x.ValidateDates(submittedViewModel.QuestionModel, content)).Returns(
             new DatesValidationResult
             {
                 StartedValidationResult = new DateValidationResult
                                           {
                                               MonthValid = true,
                                               YearValid = true
                                           },
                 AwardedValidationResult = new DateValidationResult
                                           {
                                               MonthValid = true,
                                               YearValid = true
                                           }
             }
            );

        // Act
        var result = await GetSut().QualificationDetails(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task Post_QualificationDetails_ValidModelState_ContentReturnsNull_RedirectsToError()
    {
        // Arrange
        var submittedViewModel = new QualificationDetailsPageViewModel();

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QualificationName), "Invalid");

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await controller.QualificationDetails(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help qualification details page' content could not be found");
    }

    [TestMethod]
    public async Task Post_QualificationDetails_OptionalStartDate_ValidModelStateRedirectsToProvideDetails()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
                      {
                          Heading = "Heading",
                      };

        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                        QualificationName = "Qualification name",
                                                                        QualificationAwardedDate = "5/2003",
                                                                        AwardingOrganisation =
                                                                            "Some organisation where the qualification is from"
                                                                    }
                                                                   );

        var submittedViewModel = new QualificationDetailsPageViewModel
                                 {
                                     QualificationName = "Qualification name",
                                     QuestionModel = new DatesQuestionModel
                                                     {
                                                         StartedQuestion = null,
                                                         AwardedQuestion = new()
                                                                           {
                                                                               SelectedMonth = 5,
                                                                               SelectedYear = 2003
                                                                           }
                                                     },
                                     AwardingOrganisation = "Some organisation where the qualification is from",
                                 };

        _mockHelpService.Setup(x => x.ValidateDates(submittedViewModel.QuestionModel, content)).Returns(
             new DatesValidationResult
             {
                 StartedValidationResult = null,
                 AwardedValidationResult = new DateValidationResult
                                           {
                                               MonthValid = true,
                                               YearValid = true
                                           }
             }
            );

        // Act
        var result = await GetSut().QualificationDetails(submittedViewModel);

        var enquiry = _mockHelpService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();
        enquiry.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification);

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

        _mockHelpService.Verify(x => x.SetHelpQualificationDetailsInCookie(It.IsAny<HelpFormEnquiry>(),
                                                                           submittedViewModel), Times.Once);
    }

    [TestMethod]
    public async Task
        Post_QualificationDetails_MissingRequiredFieldQualificationName_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
                      {
                          Heading = "Heading",
                      };
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                    }
                                                                   );

        var vm = new QualificationDetailsPageViewModel
                 {
                     QuestionModel = new DatesQuestionModel
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

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new(),
                                   AwardedValidationResult = new()
                               };

        _mockHelpService.Setup(x => x.ValidateDates(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>()))
                        .Returns(validationResult);

        var controller = GetSut();

        _mockHelpService
            .Setup(x => x.MapHelpQualificationDetailsPageContentToViewModel(vm, content, validationResult,
                                                                            controller.ModelState)).Returns(vm);

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
    public async Task
        Post_QualificationDetails_MissingRequiredFieldAwardingOrganisation_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        var content = new HelpQualificationDetailsPage
                      {
                          Heading = "Heading",
                      };
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                    }
                                                                   );

        var vm = new QualificationDetailsPageViewModel
                 {
                     QualificationName = "qualification name",
                     QuestionModel = new DatesQuestionModel
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
                                     }
                 };

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new(),
                                   AwardedValidationResult = new()
                               };

        _mockHelpService.Setup(x => x.ValidateDates(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>()))
                        .Returns(validationResult);

        var controller = GetSut();

        _mockHelpService
            .Setup(x => x.MapHelpQualificationDetailsPageContentToViewModel(vm, content, validationResult,
                                                                            controller.ModelState)).Returns(vm);

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
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                    }
                                                                   );

        var vm = new QualificationDetailsPageViewModel
                 {
                     QualificationName = "qualification name",
                     QuestionModel = new DatesQuestionModel
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

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new(),
                                   AwardedValidationResult = new() { MonthValid = false }
                               };

        _mockHelpService.Setup(x => x.ValidateDates(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>()))
                        .Returns(validationResult);

        _mockHelpService.Setup(x => x.HasInvalidDates(validationResult)).Returns(true);

        var controller = GetSut();

        _mockHelpService
            .Setup(x => x.MapHelpQualificationDetailsPageContentToViewModel(vm, content, validationResult,
                                                                            controller.ModelState)).Returns(vm);

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QuestionModel.AwardedQuestion
                                                .SelectedMonth), "Invalid");

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
        _mockHelpService.Setup(x => x.GetHelpQualificationDetailsPageAsync()).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(
                                                                    new HelpFormEnquiry
                                                                    {
                                                                        ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp
                                                                            .QuestionAboutAQualification,
                                                                    }
                                                                   );

        var vm = new QualificationDetailsPageViewModel
                 {
                     QualificationName = "qualification name",
                     QuestionModel = new DatesQuestionModel
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

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new(),
                                   AwardedValidationResult = new() { YearValid = false }
                               };

        _mockHelpService.Setup(x => x.ValidateDates(vm.QuestionModel, It.IsAny<HelpQualificationDetailsPage>()))
                        .Returns(validationResult);

        _mockHelpService.Setup(x => x.HasInvalidDates(validationResult)).Returns(true);

        var controller = GetSut();

        _mockHelpService
            .Setup(x => x.MapHelpQualificationDetailsPageContentToViewModel(vm, content, validationResult,
                                                                            controller.ModelState)).Returns(vm);

        // force validation error
        controller.ModelState.AddModelError(nameof(QualificationDetailsPageViewModel.QuestionModel.AwardedQuestion
                                                .SelectedYear), "Invalid");

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
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });
        _mockHelpService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().ProvideDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help provide details page' content could not be found");
    }

    [TestMethod]
    public async Task ProvideDetails_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(() => new()).Verifiable();

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().ProvideDetails();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }
[TestMethod]
public async Task ProvideDetails_ContentServiceReturnsHelpProvideDetailsPage_ReturnsProvideDetailsPageViewModel()
{
    // Arrange
    var content = new HelpProvideDetailsPage
                  {
                      Heading = "Heading",
                      BackButton = new NavigationLink
                                   {
                                       Href = "/get-help",
                                       DisplayText = "Back to get help"
                                   },
                  };

    _mockHelpService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(content);

    var enquiry = new HelpFormEnquiry
                  {
                      ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
                      AdditionalInformation = "Previously entered details",
                  };

    _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

    var viewModel = new ProvideDetailsPageViewModel
                    {
                        Heading = content.Heading,
                        BackButton = new NavigationLinkModel
                                     {
                                         DisplayText = content.BackButton.DisplayText,
                                         Href = content.BackButton.Href
                                     }
                    };

    _mockHelpService.Setup(x => x.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring))
                    .Returns(viewModel);

    // Act
    var result = await GetSut().ProvideDetails();

    // Assert
    result.Should().NotBeNull();

    var resultType = result as ViewResult;
    resultType.Should().NotBeNull();

    var model = resultType.Model as ProvideDetailsPageViewModel;
    model.Should().NotBeNull();
    model.BackButton.Should().NotBeNull();
    model.Heading.Should().Be(content.Heading);
    model.BackButton.DisplayText.Should().Be(content.BackButton.DisplayText);
    model.BackButton.Href.Should().Be(content.BackButton.Href);
    model.ProvideAdditionalInformation.Should().Be(enquiry.AdditionalInformation);
}
    [TestMethod]
    public async Task Post_ProvideDetails_ValidModelState_ContentReturnsNull_RedirectsToError()
    {
        // Arrange
        var helpForm = new HelpFormEnquiry
                       {
                           ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
                       };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var submittedViewModel = new ProvideDetailsPageViewModel();

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(ProvideDetailsPageViewModel.ProvideAdditionalInformation),
                                            "Invalid");

        _mockHelpService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await controller.ProvideDetails(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help provide details page' content could not be found");
    }

    [TestMethod]
    public async Task Post_ProvideDetails_ValidModelState_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        var submittedViewModel = new ProvideDetailsPageViewModel
                                 {
                                     ProvideAdditionalInformation = "Some details about the issue",
                                 };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().ProvideDetails(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task Post_ProvideDetails_ValidModelStateRedirectsToEmailAddress()
    {
        // Arrange
        var helpForm = new HelpFormEnquiry
                       {
                           ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
                       };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var submittedViewModel = new ProvideDetailsPageViewModel
                                 {
                                     ProvideAdditionalInformation = "Some details about the issue",
                                 };

        // Act
        var result = await GetSut().ProvideDetails(submittedViewModel);

        var enquiry = _mockHelpService.Object.GetHelpFormEnquiry();

        // Assert
        result.Should().NotBeNull();
        enquiry.Should().NotBeNull();

        enquiry.ReasonForEnquiring.Should().Be(HelpFormEnquiryReasons.GetHelp.IssueWithTheService);
        enquiry.AdditionalInformation.Should().Be("Some details about the issue");

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("EmailAddress");

        _mockHelpService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_ProvideDetails_InvalidModelState_ReturnsProvideDetailsPageViewModel()
    {
        // Arrange
        var enquiry = new HelpFormEnquiry
                      {
                          ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
                      };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(enquiry);

        var content = new HelpProvideDetailsPage
                      {
                          ErrorBannerHeading = "There is a problem",
                          AdditionalInformationErrorMessage = "additional information required"
                      };

        _mockHelpService.Setup(x => x.GetHelpProvideDetailsPage()).ReturnsAsync(content);

        var viewModel = new ProvideDetailsPageViewModel
                        {
                            AdditionalInformationErrorMessage = content.AdditionalInformationErrorMessage,
                            ErrorBannerHeading = content.ErrorBannerHeading,
                        };

        _mockHelpService.Setup(x => x.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring))
                        .Returns(viewModel);

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(ProvideDetailsPageViewModel.ProvideAdditionalInformation),
                                            "Invalid");

        // Act
        var result = await controller.ProvideDetails(viewModel);

        // Assert
        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as ProvideDetailsPageViewModel;
        model.Should().NotBeNull();

        model.HasAdditionalInformationError.Should().BeTrue();
        model.ErrorSummaryModel.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ElementLinkId.Should()
             .Be(nameof(ProvideDetailsPageViewModel.ProvideAdditionalInformation));
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ErrorBannerLinkText.Should()
             .Be(content.AdditionalInformationErrorMessage);
    }

    // Help email address tests

    [TestMethod]
    public async Task EmailAddress_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
            AdditionalInformation = "Some details"
        });
        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().EmailAddress();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help email address page' content could not be found");
    }

    [TestMethod]
    public async Task EmailAddress_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(() => new()).Verifiable();

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().EmailAddress();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task EmailAddress_EnquiryHasReasonButNoAdditionalInformation_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        // Act
        var result = await GetSut().EmailAddress();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task EmailAddress_ContentServiceReturnsHelpEmailAddressPage_ReturnsEmailAddressPageViewModel()
    {
        // Arrange
        var content = new HelpEmailAddressPage
                      {
                          Heading = "Heading",
                      };

        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(content);
        _mockHelpService.Setup(x => x.MapEmailAddressPageContentToViewModel(content)).Returns(
             new EmailAddressPageViewModel
             {
                 Heading = content.Heading,
             }
            );

        var helpForm = new HelpFormEnquiry
                       {
                           ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
                           AdditionalInformation = "Some details about the issue",
                           QualificationName = "A qualification name",
                           QualificationStartDate = "1/2000",
                           QualificationAwardedDate = "1/2001",
                           AwardingOrganisation = "An awarding organisation",
                       };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

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
        var helpForm = new HelpFormEnquiry
                       {
                           ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
                           AdditionalInformation = "Some details about the issue",
                       };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(helpForm);

        var submittedViewModel = new EmailAddressPageViewModel
                                 {
                                     EmailAddress = "test@test.com"
                                 };

        // Act
        var result = await GetSut().EmailAddress(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Confirmation");

        _mockHelpService.Verify(x => x.SendHelpPageNotification(It.IsAny<HelpPageNotification>()), Times.Once());

        _mockHelpService.Verify(x => x.SetHelpFormEnquiry(It.IsAny<HelpFormEnquiry>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_EmailAddress_ValidModelState_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        var submittedViewModel = new EmailAddressPageViewModel
                                 {
                                     EmailAddress = "test@test.com"
                                 };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().EmailAddress(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task Post_EmailAddress_EnquiryHasReasonButNoAdditionalInformation_RedirectsToGetHelpPage()
    {
        // Arrange
        var submittedViewModel = new EmailAddressPageViewModel
                                 {
                                     EmailAddress = "test@test.com"
                                 };

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        // Act
        var result = await GetSut().EmailAddress(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task Post_EmailAddress_ValidModelState_ContentReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
            AdditionalInformation = "Some details"
        });

        var submittedViewModel = new EmailAddressPageViewModel();

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(EmailAddressPageViewModel.EmailAddress), "Invalid");

        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await controller.EmailAddress(submittedViewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help email address page' content could not be found");
    }

    [TestMethod]
    public async Task Post_EmailAddress_InvalidModelState_ReturnsEmailAddressPageViewModel()
    {
        // Arrange
        var content = new HelpEmailAddressPage
                      {
                          ErrorBannerHeading = "There is a problem",
                          NoEmailAddressEnteredErrorMessage = "Enter an email address"
                      };

        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(content);

        var viewModel = new EmailAddressPageViewModel
                        {
                            EmailAddressErrorMessage = content.NoEmailAddressEnteredErrorMessage,
                            ErrorBannerHeading = content.ErrorBannerHeading,
                        };

        _mockHelpService.Setup(x => x.MapEmailAddressPageContentToViewModel(content)).Returns(viewModel);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
            AdditionalInformation = "Some details"
        });

        var controller = GetSut();

        // force validation error
        controller.ModelState.AddModelError(nameof(EmailAddressPageViewModel.EmailAddress), "Invalid");

        // Act
        var result = await controller.EmailAddress(viewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as EmailAddressPageViewModel;
        model.Should().NotBeNull();
        model.HasEmailAddressError.Should().BeTrue();

        model.ErrorSummaryModel.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ElementLinkId.Should()
             .Be(nameof(EmailAddressPageViewModel.EmailAddress));
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ErrorBannerLinkText.Should()
             .Be(content.NoEmailAddressEnteredErrorMessage);
    }

    [TestMethod]
    public async Task Post_EmailAddress_InvalidEmailFormat_ReturnsEmailAddressPageViewModelWithInvalidEmailErrorMessage()
    {
        // Arrange
        var content = new HelpEmailAddressPage
                      {
                          ErrorBannerHeading = "There is a problem",
                          NoEmailAddressEnteredErrorMessage = "Enter an email address",
                          InvalidEmailAddressErrorMessage = "Enter an email address in the correct format"
                      };

        _mockHelpService.Setup(x => x.GetHelpEmailAddressPage()).ReturnsAsync(content);

        var viewModel = new EmailAddressPageViewModel
                        {
                            EmailAddressErrorMessage = content.InvalidEmailAddressErrorMessage,
                            ErrorBannerHeading = content.ErrorBannerHeading,
                            EmailAddress = "not-a-valid-email"
                        };

        _mockHelpService.Setup(x => x.MapEmailAddressPageContentToViewModel(content)).Returns(viewModel);

        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
            AdditionalInformation = "Some details"
        });

        var controller = GetSut();

        // force validation error for invalid email format
        controller.ModelState.AddModelError(nameof(EmailAddressPageViewModel.EmailAddress), "Invalid email format");

        // Act
        var result = await controller.EmailAddress(viewModel);

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as EmailAddressPageViewModel;
        model.Should().NotBeNull();
        model.HasEmailAddressError.Should().BeTrue();

        model.EmailAddressErrorMessage.Should().Be(content.InvalidEmailAddressErrorMessage);
        model.ErrorSummaryModel.ErrorBannerHeading.Should().Be(content.ErrorBannerHeading);
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ElementLinkId.Should()
             .Be(nameof(EmailAddressPageViewModel.EmailAddress));
        model.ErrorSummaryModel.ErrorSummaryLinks.First().ErrorBannerLinkText.Should()
             .Be(content.InvalidEmailAddressErrorMessage);
    }

    // Help confirmation tests

    [TestMethod]
    public async Task Confirmation_EnquiryIsEmpty_RedirectsToGetHelpPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry());

        // Act
        var result = await GetSut().Confirmation();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("GetHelp");
        resultType.ControllerName.Should().Be("Help");

        _mockLogger.VerifyError("Help form enquiry reason is empty");
    }

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsNull_RedirectsToErrorPage()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });
        _mockHelpService.Setup(x => x.GetHelpConfirmationPage()).ReturnsAsync(() => null).Verifiable();

        // Act
        var result = await GetSut().Confirmation();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("'Help confirmation page' content could not be found");
    }

    [TestMethod]
    public async Task Confirmation_ContentServiceReturnsHelpConfirmationPage_ReturnsConfirmationPageViewModel()
    {
        // Arrange
        _mockHelpService.Setup(x => x.GetHelpFormEnquiry()).Returns(new HelpFormEnquiry
        {
            ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService
        });

        var content = new HelpConfirmationPage
                      {
                          SuccessMessage = "Message sent",
                          FeedbackComponent = new FeedbackComponent
                                              {
                                                  Body = ContentfulContentHelper.Text("Feedback body"),
                                                  Header = "Feedback heading"
                                              }
                      };

        _mockHelpService.Setup(x => x.GetHelpConfirmationPage()).ReturnsAsync(content);
        _mockHelpService.Setup(x => x.MapConfirmationPageContentToViewModelAsync(content)).Returns(Task.FromResult(
             new ConfirmationPageViewModel
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

    private HelpController GetSut()
    {
        return new HelpController(_mockLogger.Object, _mockHelpService.Object);
    }

    private void SetupStaticPage()
    {
        var content = new StaticPage
        {
            Heading = "Static page heading",
            BackButton = new NavigationLink
            {
                DisplayText = "Back to get help",
                Href = "/get-help"
            },
            Body = new()
        };

        _mockHelpService.Setup(x => x.GetStaticPage(It.IsAny<string>())).ReturnsAsync(content);

        _mockHelpService.Setup(x => x.MapStaticPage(content)).ReturnsAsync(
            new StaticPageModel
            {
                Heading = content.Heading,
                BackButton = new NavigationLinkModel
                {
                    DisplayText = content.BackButton.DisplayText,
                    Href = content.BackButton.Href
                },
                BodyContent = "content"
            }
        );
    }
}
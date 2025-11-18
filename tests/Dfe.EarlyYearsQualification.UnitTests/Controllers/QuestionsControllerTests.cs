using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Questions;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QuestionsControllerTests
{
    Mock<ILogger<QuestionsController>> _mockLogger = new Mock<ILogger<QuestionsController>>();
    Mock<IQuestionService> _mockQuestionService = new Mock<IQuestionService>();

    [TestMethod]
    public void StartNew_ResetsCookie_RedirectsToQuestionPage()
    {
        // Act
        var result = GetSut().StartNew();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be(nameof(QuestionPages.AreYouCheckingYourOwnQualification));
        _mockQuestionService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        // Arrange
        RadioQuestionPage? questionPage = null;

        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.WhereWasTheQualificationAwarded))
            .ReturnsAsync(
                questionPage
            );

        // Act
        var result = await GetSut().WhereWasTheQualificationAwarded();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
        _mockQuestionService.Verify(x => x.GetRadioQuestionPageContent(QuestionPages.WhereWasTheQualificationAwarded), Times.Once);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        // Arrange
        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue"
                           };

        var viewModel = new RadioQuestionModel
                        {
                            Question = questionPage.Question,
                            CtaButtonText = questionPage.CtaButtonText
                        };

        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.WhereWasTheQualificationAwarded))
            .ReturnsAsync(
                questionPage
            );

        _mockQuestionService.Setup(x => x.Map(It.IsAny<RadioQuestionModel>(), questionPage, nameof(QuestionsController.WhereWasTheQualificationAwarded), "Questions", It.IsAny<string>()))
            .ReturnsAsync(
                viewModel
            );

        // Act
        var result = await GetSut().WhereWasTheQualificationAwarded();

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as RadioQuestionModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_InvalidModel_ReturnsQuestionPage()
    {
        // Arrange
        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage());

        var controller = GetSut();

        controller.ModelState.AddModelError("option", "test error");

        _mockQuestionService.Setup(x => x.Map(It.IsAny<RadioQuestionModel>(),  It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new RadioQuestionModel());
        
        // Act
        var result = await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel());

        // Assert
        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Radio");

        var model = resultType.Model as RadioQuestionModel;

        model.Should().NotBeNull();
        model.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_RedirectsToAdvicePage()
    {
        // Arrange
        _mockQuestionService
            .Setup(x => x.RedirectBasedOnWhereTheQualificationWasAwarded(QualificationAwardLocation.OutsideOfTheUnitedKingdom))
            .Returns(new RedirectToActionResult(nameof(QualificationAwardLocation.OutsideOfTheUnitedKingdom), "Advice", null));

        // Act
        var result =
            await GetSut().WhereWasTheQualificationAwarded(
                new RadioQuestionModel
                {
                    Option = QualificationAwardLocation.OutsideOfTheUnitedKingdom
                }
            );

        // Assert
        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be(nameof(QualificationAwardLocation.OutsideOfTheUnitedKingdom));
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_ReturnsView()
    {
        var questionPage = new DatesQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               StartedQuestion = new DateQuestion
                                                 {
                                                     ErrorMessage = "started- Test error message",
                                                     MonthLabel = "started- Test month label",
                                                     YearLabel = "started- Test year label",
                                                     QuestionHint = "started- Test question hint"
                                                 },
                               AwardedQuestion = new DateQuestion
                                                 {
                                                     ErrorMessage = "awarded- Test error message",
                                                     MonthLabel = "awarded- Test month label",
                                                     YearLabel = "awarded- Test year label",
                                                     QuestionHint = "awarded- Test question hint"
                                                 }
                           };

        _mockQuestionService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(questionPage);

        _mockQuestionService.Setup(x => x.MapDatesModel(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), null))
                          .Returns(new DatesQuestionModel()
                          {
                              Question = questionPage.Question,
                              CtaButtonText = questionPage.CtaButtonText,
                              StartedQuestion = new DateQuestionModel
                                                {
                                                    Prefix = "started",
                                                    QuestionId = "date-started",
                                                    MonthId = "StartedQuestion.SelectedMonth",
                                                    YearId = "StartedQuestion.SelectedYear",
                                                    MonthLabel = questionPage.StartedQuestion!.MonthLabel,
                                                    YearLabel = questionPage.StartedQuestion!.YearLabel,
                                                    QuestionHint = questionPage.StartedQuestion!.QuestionHint,
                                                    ErrorMessage = questionPage.StartedQuestion!.ErrorMessage
                                                },
                              AwardedQuestion = new DateQuestionModel
                                                {
                                                    Prefix = "awarded",
                                                    QuestionId = "date-awarded",
                                                    MonthId = "AwardedQuestion.SelectedMonth",
                                                    YearId = "AwardedQuestion.SelectedYear",
                                                    MonthLabel = questionPage.AwardedQuestion!.MonthLabel,
                                                    YearLabel = questionPage.AwardedQuestion!.YearLabel,
                                                    QuestionHint = questionPage.AwardedQuestion!.QuestionHint,
                                                    ErrorMessage = questionPage.AwardedQuestion!.ErrorMessage
                                                },
                          });

        var result = await GetSut().WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as DatesQuestionModel;
        model.Should().NotBeNull();

        model.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.ErrorBannerHeading.Should().Be(string.Empty);
        model.StartedQuestion.Should().NotBeNull();
        model.AwardedQuestion.Should().NotBeNull();
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_CantFindContentfulPage_ReturnsErrorPage()
    {
        _mockQuestionService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync((DatesQuestionPage?)null).Verifiable();

        var result = await GetSut().WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_InvalidModel_ReturnsDatesQuestionPage()
    {
        var questionPage = new DatesQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorBannerHeading = "Error banner heading",
                               StartedQuestion = new DateQuestion
                                                 {
                                                     ErrorMessage = "started- Test error message",
                                                     MonthLabel = "started- Test month label",
                                                     YearLabel = "started- Test year label",
                                                     QuestionHint = "started- Test question hint"
                                                 },
                               AwardedQuestion = new DateQuestion
                                                 {
                                                     ErrorMessage = "awarded- Test error message",
                                                     MonthLabel = "awarded- Test month label",
                                                     YearLabel = "awarded- Test year label",
                                                     QuestionHint = "awarded- Test question hint"
                                                 }
                           };


        var datesQuestionModel = new DatesQuestionModel
                                 {
                                     StartedQuestion = new DateQuestionModel
                                                       {
                                                           SelectedMonth = null,
                                                           SelectedYear = null,
                                                           Prefix = "started",
                                                           QuestionId = "date-started",
                                                           MonthId = "StartedQuestion.SelectedMonth",
                                                           YearId = "StartedQuestion.SelectedYear",
                                                           MonthLabel = questionPage.StartedQuestion!.MonthLabel,
                                                           YearLabel = questionPage.StartedQuestion!.YearLabel,
                                                           QuestionHint = questionPage.StartedQuestion!.QuestionHint,
                                                           ErrorMessage = "Test error message"

                                     },
                                     AwardedQuestion = new DateQuestionModel
                                                       {
                                                           SelectedMonth = null,
                                                           SelectedYear = null,
                                                           Prefix = "awarded",
                                                           QuestionId = "date-awarded",
                                                           MonthId = "AwardedQuestion.SelectedMonth",
                                                           YearId = "AwardedQuestion.SelectedYear",
                                                           MonthLabel = questionPage.AwardedQuestion!.MonthLabel,
                                                           YearLabel = questionPage.AwardedQuestion!.YearLabel,
                                                           QuestionHint = questionPage.AwardedQuestion!.QuestionHint,
                                                           ErrorMessage = "Test error message"
                                     },
                                     Question = questionPage.Question,
                                     CtaButtonText = questionPage.CtaButtonText,
                                     Errors = new ErrorSummaryModel()
                                     {
                                         ErrorSummaryLinks = [],
                                         ErrorBannerHeading = questionPage.ErrorBannerHeading
                                     },
                                };

        var validationResult = new DatesValidationResult
                               {
                                   StartedValidationResult = new DateValidationResult
                                                             {
                                                                 MonthValid = false, YearValid = false,
                                                                 ErrorMessages = ["Test error message"],
                                                                 BannerErrorMessages = [new BannerError("Test banner error message", FieldId.Month), new BannerError("Test banner error message", FieldId.Year)]
                                                             },
                                   AwardedValidationResult = new DateValidationResult
                                                             {
                                                                 MonthValid = false, YearValid = false,
                                                                 ErrorMessages = ["Test error message"],
                                                                 BannerErrorMessages = [new BannerError("Test banner error message", FieldId.Month), new BannerError("Test banner error message", FieldId.Year)]
                                                             }
                               };

        _mockQuestionService.Setup(x => x.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded))
                          .ReturnsAsync(questionPage);

        _mockQuestionService.Setup(x => x.IsValid(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>()))
                                  .Returns(validationResult);

        _mockQuestionService.Setup(x => x.MapDatesModel(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DatesValidationResult>()))
                                  .Returns(datesQuestionModel);
    
        var result = await GetSut().WhenWasTheQualificationStarted(new DatesQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Dates");

        var model = resultType.Model as DatesQuestionModel;
        model.Should().NotBeNull();

        model.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.Errors!.ErrorBannerHeading.Should().Be(questionPage.ErrorBannerHeading);
        model.StartedQuestion!.Prefix.Should().Be("started");
        model.StartedQuestion.QuestionId.Should().Be("date-started");
        model.StartedQuestion.MonthId.Should().Be("StartedQuestion.SelectedMonth");
        model.StartedQuestion.YearId.Should().Be("StartedQuestion.SelectedYear");
        model.StartedQuestion.MonthLabel.Should().Be(questionPage.StartedQuestion.MonthLabel);
        model.StartedQuestion.YearLabel.Should().Be(questionPage.StartedQuestion.YearLabel);
        model.StartedQuestion.QuestionHint.Should().Be(questionPage.StartedQuestion.QuestionHint);
        model.StartedQuestion.ErrorMessage.Should().Be(validationResult.StartedValidationResult.ErrorMessages[0]);
        model.AwardedQuestion!.Prefix.Should().Be("awarded");
        model.AwardedQuestion.QuestionId.Should().Be("date-awarded");
        model.AwardedQuestion.MonthId.Should().Be("AwardedQuestion.SelectedMonth");
        model.AwardedQuestion.YearId.Should().Be("AwardedQuestion.SelectedYear");
        model.AwardedQuestion.MonthLabel.Should().Be(questionPage.AwardedQuestion.MonthLabel);
        model.AwardedQuestion.YearLabel.Should().Be(questionPage.AwardedQuestion.YearLabel);
        model.AwardedQuestion.QuestionHint.Should().Be(questionPage.AwardedQuestion.QuestionHint);
        model.AwardedQuestion.ErrorMessage.Should().Be(validationResult.AwardedValidationResult.ErrorMessages[0]);

        _mockQuestionService.Verify(x => x.SetWhenWasQualificationStarted(It.IsAny<DateQuestionModel>()), Times.Never);
        _mockQuestionService.Verify(x => x.SetWhenWasQualificationAwarded(It.IsAny<DateQuestionModel>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_ValidModel_ReturnsRedirectResponse()
    {
        _mockQuestionService
            .Setup(x => x.IsValid(It.IsAny<DatesQuestionModel>(), It.IsAny<DatesQuestionPage>()))
            .Returns(
                new DatesValidationResult
                {
                    StartedValidationResult = new DateValidationResult { MonthValid = true, YearValid = true },
                    AwardedValidationResult = new DateValidationResult { MonthValid = true, YearValid = true }
                }
            );

        const int startedSelectedMonth = 12;
        const int startedSelectedYear = 2024;
        const int awardedSelectedMonth = 1;
        const int awardedSelectedYear = 2025;

        var datesQuestionModel = new DatesQuestionModel
        {
            StartedQuestion = new DateQuestionModel
            {
                SelectedMonth = startedSelectedMonth,
                SelectedYear = startedSelectedYear
            },
            AwardedQuestion = new DateQuestionModel
            {
                SelectedMonth = awardedSelectedMonth,
                SelectedYear = awardedSelectedYear
            }
        };

        var result = await GetSut().WhenWasTheQualificationStarted(datesQuestionModel);

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("WhatLevelIsTheQualification");

        _mockQuestionService
            .Verify(x => x.SetWhenWasQualificationStarted(datesQuestionModel.StartedQuestion),
                    Times.Once);
        _mockQuestionService
            .Verify(x => x.SetWhenWasQualificationAwarded(datesQuestionModel.AwardedQuestion),
                    Times.Once);
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        _mockQuestionService.Setup(x => x.GetLevelOfQualification()).Returns(3);

        RadioQuestionPage? questionPage = null;

        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification))
            .ReturnsAsync(
                questionPage
            );

        var result = await GetSut().WhatLevelIsTheQualification();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockQuestionService.Verify(x => x.GetLevelOfQualification(), Times.Once);
        _mockQuestionService.Verify(x => x.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification), Times.Once);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_InvalidModel_ReturnsQuestionPage()
    {
        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage());

        var controller = GetSut();

        controller.ModelState.AddModelError("option", "test error");

        _mockQuestionService.Setup(x => x.Map(It.IsAny<RadioQuestionModel>(),  It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new RadioQuestionModel());
        
        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();
        resultType.ViewName.Should().Be("Radio");

        var model = resultType.Model as RadioQuestionModel;
        model.Should().NotBeNull();
        model.HasErrors.Should().BeTrue();

        _mockQuestionService.Verify(x => x.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification), Times.Once);
        _mockQuestionService.Verify(x => x.Map(It.IsAny<RadioQuestionModel>(), It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _mockQuestionService.Verify(x => x.RedirectBasedOnQualificationLevelSelected("2"), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_ReturnsRedirectResponse()
    {
        var level = "2";
        _mockQuestionService.Setup(x => x.RedirectBasedOnQualificationLevelSelected(level)).Returns(new Mock<IActionResult>().Object);

        var result = await GetSut().WhatLevelIsTheQualification(
            new RadioQuestionModel
            {
                Option = level
            }
        );

        result.Should().NotBeNull();
        _mockQuestionService.Verify(x => x.RedirectBasedOnQualificationLevelSelected(level), Times.Once);
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        _mockQuestionService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync((DropdownQuestionPage?)null).Verifiable();

        var result = await GetSut().WhatIsTheAwardingOrganisation();

        _mockQuestionService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        _mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        _mockQuestionService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        _mockQuestionService.Setup(x => x.MapDropdownModel(It.IsAny<DropdownQuestionModel>(), It.IsAny<DropdownQuestionPage>(), It.IsAny<List<Qualification>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                  .ReturnsAsync(new DropdownQuestionModel());

        var result = await GetSut().WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as DropdownQuestionModel;
        model.Should().NotBeNull();
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsQualifications_OrdersAwardingOrganisationsInModel()
    {
        var questionPage = new DropdownQuestionPage
                           {
                               DefaultText = "Test default text"
                           };

        _mockQuestionService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        var listOfQualifications = new List<Qualification>
                                   {
                                       new("1", "TEST",
                                           "D awarding organisation", 123),
                                       new("2", "TEST",
                                           "E awarding organisation", 123),
                                       new("3", "TEST",
                                           "A awarding organisation", 123),
                                       new("4", "TEST",
                                           "C awarding organisation", 123),
                                       new("5", "TEST",
                                           "B awarding organisation", 123)
                                   };

        _mockQuestionService.Setup(x => x.GetFilteredQualifications()).ReturnsAsync(listOfQualifications);

        _mockQuestionService.Setup(x => x.MapDropdownModel(It.IsAny<DropdownQuestionModel>(), It.IsAny<DropdownQuestionPage>(), listOfQualifications, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                  .ReturnsAsync(new DropdownQuestionModel());

        var result = await GetSut().WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as DropdownQuestionModel;
        model.Should().NotBeNull();
        model.Values.Should().NotBeNull();
    }

    [TestMethod]
    public async Task
        WhatIsTheAwardingOrganisation_ContentServiceReturnsQualificationsWithVariousOrHigherEducation_FiltersThemOutOfResponse()
    {
        var questionPage = new DropdownQuestionPage
                           {
                               DefaultText = "Test default text"
                           };

        _mockQuestionService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        var listOfQualifications = new List<Qualification>
                                   {
                                       new("1", "TEST",
                                           "D awarding organisation", 123),
                                       new("2", "TEST",
                                           "E awarding organisation", 123),
                                       new("3", "TEST",
                                           AwardingOrganisations.Various, 123),
                                       new("4", "TEST",
                                           AwardingOrganisations.AllHigherEducation, 123)
                                   };

        _mockQuestionService.Setup(x => x.GetFilteredQualifications()).ReturnsAsync(listOfQualifications);

        _mockQuestionService.Setup(x => x.MapDropdownModel(It.IsAny<DropdownQuestionModel>(), It.IsAny<DropdownQuestionPage>(), listOfQualifications, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                  .ReturnsAsync(new DropdownQuestionModel());

        var result = await GetSut().WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType.Model as DropdownQuestionModel;
        model.Should().NotBeNull();
        model.Values.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Post_WhatIsTheAwardingOrganisation_InvalidModel_ReturnsQuestionPage()
    {
        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        _mockQuestionService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        var controller = GetSut();

        controller.ModelState.AddModelError("option", "test error");

        _mockQuestionService.Setup(x => x.MapDropdownModel(It.IsAny<DropdownQuestionModel>(), It.IsAny<DropdownQuestionPage>(), It.IsAny<List<Qualification>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                                  .ReturnsAsync(new DropdownQuestionModel());
        
        var result = await controller.WhatIsTheAwardingOrganisation(new DropdownQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Dropdown");

        var model = resultType.Model as DropdownQuestionModel;
        model.Should().NotBeNull();

        model.HasErrors.Should().BeTrue();
    }

    [TestMethod]
    public async Task Post_WhatIsTheAwardingOrganisation_NoValueSelectedAndNotInListNotSelected_ReturnsQuestionPage()
    {
        var result = await GetSut().WhatIsTheAwardingOrganisation(
            new DropdownQuestionModel
            {
                SelectedValue = string.Empty,
                NotInTheList = false
            }
        );

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Dropdown");
    }

    [TestMethod]
    public async Task Post_WhatIsTheAwardingOrganisation_ValidSubmit_ReturnsRedirectToActionResult()
    {
        var result = await GetSut().WhatIsTheAwardingOrganisation(
            new DropdownQuestionModel
            {
                SelectedValue = "some value",
                NotInTheList = false
            }
        );

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();
        resultType.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("CheckYourAnswers");
    }

    [TestMethod]
    public async Task PreCheck_UnableToGetContent_ReturnsErrorPage()
    {
        _mockQuestionService.Setup(x => x.GetPreCheckView())
            .ReturnsAsync(new RedirectToActionResult("Index", "Error", null));

        var result = await GetSut().PreCheck();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");
    }
    
    [TestMethod]
    public async Task PreCheck_FindsContent_ReturnsPage()
    {
        _mockQuestionService.Setup(x => x.GetPreCheckView())
            .ReturnsAsync(
                new ViewResult()
                {
                    ViewName = "PreCheck",
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = new PreCheckPageModel()
                    }
                }
            );

        var result = await GetSut().PreCheck();

        _mockQuestionService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("PreCheck");
        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<PreCheckPageModel>();

        _mockQuestionService.Verify(x => x.GetPreCheckView(), Times.Once);
    }
    
    [TestMethod]
    public async Task Post_PreCheck_ModelStateInvalid_ReturnsPage()
    {
        var controller = GetSut();

        _mockQuestionService.Setup(x => x.GetPreCheckPage())
                          .ReturnsAsync(new PreCheckPage()).Verifiable();

        controller.ModelState.AddModelError("option", "test error");

        _mockQuestionService.Setup(x => x.MapPreCheckModel(It.IsAny<PreCheckPage>())).ReturnsAsync(new PreCheckPageModel());

        var result = await controller.PreCheck(new PreCheckPageModel());

        _mockQuestionService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("PreCheck");
        resultType.Model.Should().NotBeNull();
        resultType.Model.Should().BeAssignableTo<PreCheckPageModel>();
    }
    
    [TestMethod]
    public async Task Post_PreCheck_UserChoosesYesOption_RedirectsToNextQuestion()
    {
        var result = await GetSut().PreCheck(new PreCheckPageModel { Option = Options.Yes });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be(nameof(QuestionsController.StartNew));
    }
    
    [TestMethod]
    public async Task Post_PreCheck_UserChoosesNoOption_RedirectsToHomePage()
    {
        var result = await GetSut().PreCheck(new PreCheckPageModel { Option = Options.No });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be(nameof(HomeController.Index));
        resultType.ControllerName.Should().Be("Home");
    }
    
    [TestMethod]
    public async Task AreYouCheckingYourOwnQualification_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        // Arrange
        var questionPage = new RadioQuestionPage
        {
            Question = "Test question",
            CtaButtonText = "Continue"
        };

        var viewModel = new RadioQuestionModel
        {
            Question = questionPage.Question,
            CtaButtonText = questionPage.CtaButtonText
        };

        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.AreYouCheckingYourOwnQualification))
            .ReturnsAsync(
                questionPage
            );

        _mockQuestionService.Setup(x => x.Map(It.IsAny<RadioQuestionModel>(), questionPage, nameof(QuestionsController.AreYouCheckingYourOwnQualification), "Questions", It.IsAny<string>()))
            .ReturnsAsync(
                viewModel
            );

        var result = await GetSut().AreYouCheckingYourOwnQualification();

        result.Should().NotBeNull();
        _mockQuestionService.Verify(x => x.GetIsUserCheckingTheirOwnQualification(), Times.Once);
        _mockQuestionService.Verify(x => x.GetRadioQuestionPageContent(QuestionPages.AreYouCheckingYourOwnQualification), Times.Once);
    }

    [TestMethod]
    public async Task Post_AreYouCheckingYourOwnQualification_InvalidModel_ReturnsQuestionPage()
    {
        var controller = GetSut();

        _mockQuestionService.Setup(x => x.GetRadioQuestionPageContent(QuestionPages.AreYouCheckingYourOwnQualification))
                          .ReturnsAsync(new RadioQuestionPage());

        controller.ModelState.AddModelError("option", "test error");

        _mockQuestionService.Setup(x => x.Map(It.IsAny<RadioQuestionModel>(),  It.IsAny<RadioQuestionPage>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new  RadioQuestionModel());
        
        var result = await controller.AreYouCheckingYourOwnQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType.ViewName.Should().Be("Radio");

        var model = resultType.Model as RadioQuestionModel;

        model.Should().NotBeNull();
        model.HasErrors.Should().BeTrue();

        _mockQuestionService.Verify(x => x.SetIsUserCheckingTheirOwnQualification(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_AreYouCheckingYourOwnQualification_ReturnsRedirectResponse()
    {
        var result = await GetSut().AreYouCheckingYourOwnQualification(
            new RadioQuestionModel
            {
                Option = "yes"
            }
        );

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType.ActionName.Should().Be("WhereWasTheQualificationAwarded");

        _mockQuestionService.Verify(x => x.SetIsUserCheckingTheirOwnQualification("yes"), Times.Once);
    }

    private QuestionsController GetSut()
    {
        return new QuestionsController(
                                _mockLogger.Object,
                                _mockQuestionService.Object
                                );
    }
}
using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QuestionsControllerTests
{
    [TestMethod]
    public void StartNew_ResetsCookie_RedirectsToQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = controller.StartNew();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhereWasTheQualificationAwarded");

        mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }]
                           };
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.OptionsItems.Count.Should().Be(1);
        model.OptionsItems[0].Should().BeAssignableTo<OptionModel>();
        var modelOption = model.OptionsItems[0] as OptionModel;
        modelOption!.Label.Should().Be((questionPage.Options[0] as Option)!.Label);
        modelOption.Value.Should().Be((questionPage.Options[0] as Option)!.Value);
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_InvalidModel_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(new RadioQuestionPage());

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");

        var model = resultType.Model as RadioQuestionModel;

        model.Should().NotBeNull();
        model!.HasErrors.Should().BeTrue();

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             {
                                                                 Option = QualificationAwardLocation
                                                                     .OutsideOfTheUnitedKingdom
                                                             });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationOutsideTheUnitedKingdom");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService
            .Verify(x => x.SetWhereWasQualificationAwarded(QualificationAwardLocation.OutsideOfTheUnitedKingdom),
                    Times.Once);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInScotland_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.Scotland });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInScotland");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(QualificationAwardLocation.Scotland),
                                            Times.Once);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInWales_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.Wales });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInWales");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(QualificationAwardLocation.Wales),
                                            Times.Once);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInNorthernIreland_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.NorthernIreland });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInNorthernIreland");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService
            .Verify(x => x.SetWhereWasQualificationAwarded(QualificationAwardLocation.NorthernIreland),
                    Times.Once);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInEngland_RedirectsToQualificationDetails()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.England });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhenWasTheQualificationStarted");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(QualificationAwardLocation.England),
                                            Times.Once);
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new DateQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               MonthLabel = "Test month label",
                               YearLabel = "Test year label",
                               QuestionHint = "Test question hint"
                           };
        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync(questionPage);

        mockPlaceholderUpdater.Setup(x => x.Replace(It.IsAny<string>())).Returns<string>(x => x);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model.MonthLabel.Should().Be(questionPage.MonthLabel);
        model.YearLabel.Should().Be(questionPage.YearLabel);
        model.QuestionHint.Should().Be(questionPage.QuestionHint);
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_CantFindContentfulPage_ReturnsErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync((DateQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_InvalidModel_ReturnsDateQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new DateQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               MonthLabel = "Test month label",
                               YearLabel = "Test year label",
                               QuestionHint = "Test question hint"
                           };
        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        mockQuestionModelValidator.Setup(x => x.IsValid(It.IsAny<DateQuestionModel>(), It.IsAny<DateQuestionPage>()))
                                  .Returns(new DateValidationResult
                                           { MonthValid = false, YearValid = false, ErrorMessages = ["Test error message"] });

        mockPlaceholderUpdater.Setup(x => x.Replace(It.IsAny<string>())).Returns<string>(x => x);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Date");

        var model = resultType.Model as DateQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model.MonthLabel.Should().Be(questionPage.MonthLabel);
        model.YearLabel.Should().Be(questionPage.YearLabel);
        model.QuestionHint.Should().Be(questionPage.QuestionHint);

        mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationStarted(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_ValidModel_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockQuestionModelValidator
            .Setup(x => x.IsValid(It.IsAny<DateQuestionModel>(), It.IsAny<DateQuestionPage>()))
            .Returns(new DateValidationResult { MonthValid = true, YearValid = true, });

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        const int selectedMonth = 12;
        const int selectedYear = 2024;

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel
                                                                     {
                                                                         SelectedMonth = selectedMonth,
                                                                         SelectedYear = selectedYear
                                                                     });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhatLevelIsTheQualification");

        mockUserJourneyCookieService
            .Verify(x => x.SetWhenWasQualificationStarted($"{selectedMonth}/{selectedYear}"),
                    Times.Once);
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatLevelIsTheQualification();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options =
                               [
                                   new Option { Label = "Label", Value = "Value" },
                                   new Divider { Text = "Test" }
                               ],
                               AdditionalInformationHeader = "Test header",
                               AdditionalInformationBody = ContentfulContentHelper.Text("Test html body")
                           };
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(questionPage);

        mockContentParser.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatLevelIsTheQualification();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.OptionsItems.Count.Should().Be(2);
        model.OptionsItems[0].Should().BeAssignableTo<OptionModel>();
        var modelOption = model.OptionsItems[0] as OptionModel;
        modelOption!.Label.Should().Be((questionPage.Options[0] as Option)!.Label);
        modelOption.Value.Should().Be((questionPage.Options[0] as Option)!.Value);
        model.OptionsItems[1].Should().BeAssignableTo<DividerModel>();
        var dividerOption = model.OptionsItems[1] as DividerModel;
        dividerOption!.Text.Should().Be((questionPage.Options[1] as Divider)!.Text);
        model.HasErrors.Should().BeFalse();
        model.AdditionalInformationHeader.Should().Be(questionPage.AdditionalInformationHeader);
        model.AdditionalInformationBody.Should().Be("Test html body");

        mockContentParser.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_InvalidModel_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(new RadioQuestionPage());

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");

        var model = resultType.Model as RadioQuestionModel;

        model.Should().NotBeNull();
        model!.HasErrors.Should().BeTrue();

        mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "3"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhatIsTheAwardingOrganisation");

        mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification("3"), Times.Once);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_Level2StartedBetween2014And2019_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockUserJourneyCookieService.Setup(x => x.WasStartedBetweenSeptember2014AndAugust2019())
                                    .Returns(true);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "2"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsStartedBetweenSept2014AndAug2019");
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_Level7Post2014_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockUserJourneyCookieService.Setup(x => x.WasStartedOnOrAfterSeptember2014())
                                    .Returns(true);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "7"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Level7QualificationPost2014");
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync((DropdownQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatIsTheAwardingOrganisation();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Index");
        resultType.ControllerName.Should().Be("Error");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        mockRepository
            .Setup(x => x.Get(
                              It.IsAny<int?>(),
                              It.IsAny<int?>(),
                              It.IsAny<int?>(),
                              It.IsAny<string?>(),
                              It.IsAny<string?>()))
            .ReturnsAsync([]);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DropdownQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model.DropdownHeading.Should().Be(questionPage.DropdownHeading);
        model.HasErrors.Should().BeFalse();
        model.Values.Count.Should().Be(1);
        model.Values[0].Text.Should().Be(questionPage.DefaultText);
        model.Values[0].Value.Should().BeEmpty();
        model.NotInListText.Should().Be(questionPage.NotInListText);
    }

    [TestMethod]
    public async Task
        WhatIsTheAwardingOrganisation_ContentServiceReturnsQualifications_OrdersAwardingOrganisationsInModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new DropdownQuestionPage
                           {
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
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

        mockRepository
            .Setup(x => x.Get(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                              It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(listOfQualifications);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DropdownQuestionModel;
        model.Should().NotBeNull();
        model!.Values.Should().NotBeNull();

        // Count here includes default value added in mapping
        model.Values.Count.Should().Be(6);
        model.Values[0].Text.Should().Be(questionPage.DefaultText);
        model.Values[0].Value.Should().BeEmpty();

        model.Values[1].Text.Should().Be("A awarding organisation");
        model.Values[2].Text.Should().Be("B awarding organisation");
        model.Values[3].Text.Should().Be("C awarding organisation");
        model.Values[4].Text.Should().Be("D awarding organisation");
        model.Values[5].Text.Should().Be("E awarding organisation");
    }

    [TestMethod]
    public async Task
        WhatIsTheAwardingOrganisation_ContentServiceReturnsQualificationsWithVariousOrHigherEducation_FiltersThemOutOfResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var questionPage = new DropdownQuestionPage
                           {
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
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

        mockRepository
            .Setup(x => x.Get(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                              It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(listOfQualifications);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatIsTheAwardingOrganisation();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DropdownQuestionModel;
        model.Should().NotBeNull();
        model!.Values.Should().NotBeNull();

        // Count here includes default value added in mapping
        model.Values.Count.Should().Be(3);
        model.Values[1].Text.Should().Be("D awarding organisation");
        model.Values[2].Text.Should().Be("E awarding organisation");
    }

    [TestMethod]
    public async Task Post_WhatIsTheAwardingOrganisation_InvalidModel_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        mockRepository
            .Setup(x => x.Get(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                              It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync([]);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhatIsTheAwardingOrganisation(new DropdownQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Dropdown");

        var model = resultType.Model as DropdownQuestionModel;
        model.Should().NotBeNull();

        model!.HasErrors.Should().BeTrue();

        mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisation(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhatIsTheAwardingOrganisation_NoValueSelectedAndNotInListNotSelected_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var result = await controller.WhatIsTheAwardingOrganisation(new DropdownQuestionModel
                                                                    {
                                                                        SelectedValue = string.Empty,
                                                                        NotInTheList = false
                                                                    });

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Dropdown");

        mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisation(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task
        Post_WhatIsTheAwardingOrganisation_AwardingOrgPassedIn_SetsJourneyCookieAndRedirectsToTheQualificationListPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        var result = await controller.WhatIsTheAwardingOrganisation(new DropdownQuestionModel
                                                                    {
                                                                        SelectedValue = "Some Awarding Organisation",
                                                                        NotInTheList = false
                                                                    });

        result.Should().NotBeNull();

        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Get");
        resultType.ControllerName.Should().Be("QualificationSearch");

        mockUserJourneyCookieService
            .Verify(x => x.SetAwardingOrganisation("Some Awarding Organisation"), Times.Once);
    }

    [TestMethod]
    public async Task
        Post_WhatIsTheAwardingOrganisation_NotInTheListPassedIn_SetsJourneyCookieAndRedirectsToTheQualificationListPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockContentParser = new Mock<IGovUkContentParser>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockRepository = new Mock<IQualificationsRepository>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();
        var mockPlaceholderUpdater = new Mock<IPlaceholderUpdater>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockContentParser.Object,
                                                 mockUserJourneyCookieService.Object, mockRepository.Object,
                                                 mockQuestionModelValidator.Object, mockPlaceholderUpdater.Object);

        var questionPage = new DropdownQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               DropdownHeading = "Test dropdown heading",
                               NotInListText = "Test not in the list text",
                               DefaultText = "Test default text"
                           };

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync(questionPage);

        var result = await controller.WhatIsTheAwardingOrganisation(new DropdownQuestionModel
                                                                    {
                                                                        SelectedValue = "",
                                                                        NotInTheList = true
                                                                    });

        result.Should().NotBeNull();

        result.Should().NotBeNull();
        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Get");
        resultType.ControllerName.Should().Be("QualificationSearch");

        mockUserJourneyCookieService
            .Verify(x => x.SetAwardingOrganisation(string.Empty), Times.Once);
    }
}
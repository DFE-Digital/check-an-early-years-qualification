using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models;
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
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

        mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }]
                           };
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

        mockUserJourneyCookieService.Verify(x => x.ResetUserJourneyCookie(), Times.Once);

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.Options.Count.Should().Be(1);
        model.Options[0].Label.Should().Be(questionPage.Options[0].Label);
        model.Options[0].Value.Should().Be(questionPage.Options[0].Value);
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_InvalidModel_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInScotland_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.Scotland });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInScotland");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInWales_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.Wales });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInWales");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInNorthernIreland_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = QualificationAwardLocation.NorthernIreland });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationsAchievedInNorthernIreland");
        resultType.ControllerName.Should().Be("Advice");

        mockUserJourneyCookieService.Verify(x => x.SetWhereWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInEngland_RedirectsToQualificationDetails()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        // The following will need to be replaced once the page has been created in Contentful.
        // The model is currently hard coded in the action and doesn't call the content service.
        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.HasErrors.Should().BeFalse();
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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync((DateQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Date");

        mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    [DataRow(-1, 2020)]
    [DataRow(13, 2020)]
    [DataRow(0, 2020)]
    [DataRow(1, 1899)]
    public async Task Post_WhenWasTheQualificationStarted_PassedInvalidValues_ReturnsBackToPageWithErrorTag(
        int month, int year)
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel
                                                                     {
                                                                         SelectedMonth = month,
                                                                         SelectedYear = year
                                                                     });

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.HasErrors.Should().BeTrue();
        model.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model.MonthLabel.Should().Be(questionPage.MonthLabel);
        model.YearLabel.Should().Be(questionPage.YearLabel);
        model.QuestionHint.Should().Be(questionPage.QuestionHint);

        mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_YearProvidedIsNextYear_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel
                                                                     {
                                                                         SelectedMonth = 01,
                                                                         SelectedYear = DateTime.UtcNow.Year + 1
                                                                     });

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.HasErrors.Should().BeTrue();
        model.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model.MonthLabel.Should().Be(questionPage.MonthLabel);
        model.YearLabel.Should().Be(questionPage.YearLabel);
        model.QuestionHint.Should().Be(questionPage.QuestionHint);

        mockUserJourneyCookieService.Verify(x => x.SetWhenWasQualificationAwarded(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_ValidModel_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockQuestionModelValidator
            .Setup(x => x.IsValid(It.IsAny<DateQuestionModel>()))
            .Returns(true);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
            .Verify(x => x.SetWhenWasQualificationAwarded($"{selectedMonth}/{selectedYear}"),
                    Times.Once);
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }],
                               AdditionalInformationHeader = "Test header",
                               AdditionalInformationBody = ContentfulContentHelper.Text("Test html body")
                           };
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync(questionPage);

        mockRenderer.Setup(x => x.ToHtml(It.IsAny<Document>())).ReturnsAsync("Test html body");

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhatLevelIsTheQualification();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as RadioQuestionModel;
        model.Should().NotBeNull();

        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.Options.Count.Should().Be(1);
        model.Options[0].Label.Should().Be(questionPage.Options[0].Label);
        model.Options[0].Value.Should().Be(questionPage.Options[0].Value);
        model.HasErrors.Should().BeFalse();
        model.AdditionalInformationHeader.Should().Be(questionPage.AdditionalInformationHeader);
        model.AdditionalInformationBody.Should().Be("Test html body");

        mockRenderer.Verify(x => x.ToHtml(It.IsAny<Document>()), Times.Once);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_InvalidModel_ReturnsQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");

        mockUserJourneyCookieService.Verify(x => x.SetLevelOfQualification(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
    public async Task Post_WhatLevelIsTheQualification_Level2WithInDate_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationAwarded())
                                    .Returns((6, 2015));
        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
    public async Task Post_WhatLevelIsTheQualification_Level6Pre2014_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationAwarded())
                                    .Returns((7, 2014));
        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "6"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Level6QualificationPre2014");
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_Level6Post2014_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationAwarded())
                                    .Returns((7, 2015));
        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "6"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Level6QualificationPost2014");
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_Level7_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockUserJourneyCookieService.Setup(x => x.GetWhenWasQualificationAwarded())
                                    .Returns((6, 2015));
        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel
                                                                  {
                                                                      Option = "7"
                                                                  });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be(nameof(AdviceController.QualificationLevel7));
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task WhatIsTheAwardingOrganisation_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        mockContentService.Setup(x => x.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation))
                          .ReturnsAsync((DropdownQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        mockUserJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel());
        mockContentFilterService
            .Setup(x => x.GetFilteredQualifications(
                                                    It.IsAny<int?>(),
                                                    It.IsAny<int?>(),
                                                    It.IsAny<int?>(),
                                                    It.IsAny<string?>(),
                                                    It.IsAny<string?>()))
            .ReturnsAsync([]);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        mockUserJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel());
        mockContentFilterService
            .Setup(x => x.GetFilteredQualifications(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                                                    It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(listOfQualifications);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

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

        mockUserJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel());
        mockContentFilterService
            .Setup(x => x.GetFilteredQualifications(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                                                    It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(listOfQualifications);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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

        mockUserJourneyCookieService.Setup(x => x.GetUserJourneyModelFromCookie()).Returns(new UserJourneyModel());
        mockContentFilterService
            .Setup(x => x.GetFilteredQualifications(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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

        mockContentService.Setup(x => x.GetQualifications()).ReturnsAsync([]);

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
        resultType.ControllerName.Should().Be("QualificationDetails");

        mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisation("Some Awarding Organisation"), Times.Once);
    }

    [TestMethod]
    public async Task
        Post_WhatIsTheAwardingOrganisation_NotInTheListPassedIn_DoesNotSetsJourneyCookieAndRedirectsToTheQualificationListPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();
        var mockUserJourneyCookieService = new Mock<IUserJourneyCookieService>();
        var mockContentFilterService = new Mock<IContentFilterService>();
        var mockQuestionModelValidator = new Mock<IDateQuestionModelValidator>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object,
                                                 mockUserJourneyCookieService.Object, mockContentFilterService.Object,
                                                 mockQuestionModelValidator.Object);

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

        mockContentService.Setup(x => x.GetQualifications()).ReturnsAsync([]);

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
        resultType.ControllerName.Should().Be("QualificationDetails");

        mockUserJourneyCookieService.Verify(x => x.SetAwardingOrganisation(string.Empty), Times.Once);
    }
}
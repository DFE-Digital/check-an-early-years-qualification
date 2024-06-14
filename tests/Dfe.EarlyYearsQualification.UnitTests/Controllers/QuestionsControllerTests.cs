using Contentful.Core.Models;
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Mock.Helpers;
using Dfe.EarlyYearsQualification.UnitTests.Extensions;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
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

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var questionPage = new RadioQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }]
                           };
        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhereWasTheQualificationAwarded();

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result =
            await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel
                                                             { Option = Options.OutsideOfTheUnitedKingdom });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("QualificationOutsideTheUnitedKingdom");
        resultType.ControllerName.Should().Be("Advice");
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInEngland_RedirectsToQualificationDetails()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhereWasTheQualificationAwarded(new RadioQuestionModel { Option = Options.England });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhenWasTheQualificationStarted");
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var questionPage = new DateQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               MonthLabel = "Test month label",
                               YearLabel = "Test year label",
                               QuestionHint = "Test quesiton hint"
                           };
        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

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
        model!.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model!.MonthLabel.Should().Be(questionPage.MonthLabel);
        model!.YearLabel.Should().Be(questionPage.YearLabel);
        model!.QuestionHint.Should().Be(questionPage.QuestionHint);
    }

    [TestMethod]
    public async Task WhenWasTheQualificationStarted_CantFindContentfulPage_ReturnsErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync((DateQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_InvalidModel_ReturnsDateQuestionPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Date");
    }

    [TestMethod]
    [DataRow(-1, 2020)]
    [DataRow(13, 2020)]
    [DataRow(0, 2020)]
    [DataRow(1, 1899)]
    public async Task Post_WhenWasTheQualificationStarted_PassedInvalidValues_ReturnsBackToPageWithErrorTag(int month, int year)
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var questionPage = new DateQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               MonthLabel = "Test month label",
                               YearLabel = "Test year label",
                               QuestionHint = "Test quesiton hint"
                           };
        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel()
        {
          SelectedMonth = month,
          SelectedYear = year
        });

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        // The following will need to be replaced once the page has been created in Contentful.
        // The model is currently hard coded in the action and doesn't call the content service.
        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.HasErrors.Should().BeTrue();
        model!.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model!.MonthLabel.Should().Be(questionPage.MonthLabel);
        model!.YearLabel.Should().Be(questionPage.YearLabel);
        model!.QuestionHint.Should().Be(questionPage.QuestionHint);
    }

     public async Task Post_WhenWasTheQualificationStarted_YearProvidedIsNextYear_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var questionPage = new DateQuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               ErrorMessage = "Test error message",
                               MonthLabel = "Test month label",
                               YearLabel = "Test year label",
                               QuestionHint = "Test quesiton hint"
                           };
        mockContentService.Setup(x => x.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted))
                          .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel()
        {
          SelectedMonth = 01,
          SelectedYear = DateTime.UtcNow.Year + 1
        });

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as DateQuestionModel;
        model.Should().NotBeNull();

        // The following will need to be replaced once the page has been created in Contentful.
        // The model is currently hard coded in the action and doesn't call the content service.
        model!.Question.Should().Be(questionPage.Question);
        model.CtaButtonText.Should().Be(questionPage.CtaButtonText);
        model.HasErrors.Should().BeTrue();
        model!.ErrorMessage.Should().Be(questionPage.ErrorMessage);
        model!.MonthLabel.Should().Be(questionPage.MonthLabel);
        model!.YearLabel.Should().Be(questionPage.YearLabel);
        model!.QuestionHint.Should().Be(questionPage.QuestionHint);
    }

    [TestMethod]
    public async Task Post_WhenWasTheQualificationStarted_ValidModel_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhenWasTheQualificationStarted(new DateQuestionModel()
        {
          SelectedMonth = 12,
          SelectedYear = 2024
        });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhatLevelIsTheQualification");
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        mockContentService.Setup(x => x.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification))
                          .ReturnsAsync((RadioQuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhatLevelIsTheQualification();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhatLevelIsTheQualification_ContentServiceReturnsQuestionPage_ReturnsQuestionModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        controller.ModelState.AddModelError("option", "test error");
        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Radio");
    }

    [TestMethod]
    public async Task Post_WhatLevelIsTheQualification_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        var mockRenderer = new Mock<IHtmlRenderer>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object, mockRenderer.Object);

        var result = await controller.WhatLevelIsTheQualification(new RadioQuestionModel());

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Get");
        resultType.ControllerName.Should().Be("QualificationDetails");
    }
}
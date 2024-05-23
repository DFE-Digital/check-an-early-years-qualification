using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
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
       

        mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                           .ReturnsAsync((QuestionPage?)default).Verifiable();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result = await controller!.WhereWasTheQualificationAwarded();

        mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");

        mockLogger.VerifyError("No content for the question page");
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsAdvicePageModel()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();
        
        var questionPage = new QuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }]
                           };
        mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                           .ReturnsAsync(questionPage);

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result = await controller!.WhereWasTheQualificationAwarded();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QuestionModel;
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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        controller!.ModelState.AddModelError("option", "test error");
        var result = await controller!.WhereWasTheQualificationAwarded(new QuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Question");
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result =
            await controller!.WhereWasTheQualificationAwarded(new QuestionModel
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

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result = await controller!.WhereWasTheQualificationAwarded(new QuestionModel { Option = Options.England });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("WhenWasTheQualificationStarted");
    }

    [TestMethod]
    public void WhenWasTheQualificationStarted_ReturnsView()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result = controller!.WhenWasTheQualificationStarted();

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        var model = resultType!.Model as QuestionModel;
        model.Should().NotBeNull();

        // TODO: The following will need to be replaced once the page has been created in Contentful.
        // The model is currently hard coded in the action and doesn't call the content service.
        model!.Question.Should().Be("When was the qualification started?");
        model!.CtaButtonText.Should().Be("Continue");
        model.HasErrors.Should().BeFalse();
    }

    [TestMethod]
    public void Post_WhenWasTheQualificationStarted_ReturnsRedirectResponse()
    {
        var mockLogger = new Mock<ILogger<QuestionsController>>();
        var mockContentService = new Mock<IContentService>();

        var controller = new QuestionsController(mockLogger.Object, mockContentService.Object);

        var result = controller!.WhenWasTheQualificationStarted(new QuestionModel());

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Get");
        resultType.ControllerName.Should().Be("QualificationDetails");
    }
}
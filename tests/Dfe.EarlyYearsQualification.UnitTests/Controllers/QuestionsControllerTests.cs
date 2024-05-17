using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QuestionsControllerTests
{
    private readonly ILogger<QuestionsController> _mockLogger =
        new NullLoggerFactory().CreateLogger<QuestionsController>();

    private QuestionsController? _controller;
    private Mock<IContentService> _mockContentService = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new QuestionsController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        _mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                           .ReturnsAsync((QuestionPage?)default).Verifiable();

        var result = await _controller!.WhereWasTheQualificationAwarded();

        _mockContentService.VerifyAll();

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        result.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Error");
        resultType.ControllerName.Should().Be("Home");
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsAdvicePageModel()
    {
        var questionPage = new QuestionPage
                           {
                               Question = "Test question",
                               CtaButtonText = "Continue",
                               Options = [new Option { Label = "Label", Value = "Value" }]
                           };
        _mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded))
                           .ReturnsAsync(questionPage);
        var result = await _controller!.WhereWasTheQualificationAwarded();

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
        _controller!.ModelState.AddModelError("option", "test error");
        var result = await _controller!.WhereWasTheQualificationAwarded(new QuestionModel());

        result.Should().NotBeNull();

        var resultType = result as ViewResult;
        resultType.Should().NotBeNull();

        resultType!.ViewName.Should().Be("Question");
    }

    [TestMethod]
    public async Task Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var result =
            await _controller!.WhereWasTheQualificationAwarded(new QuestionModel
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
        var result = await _controller!.WhereWasTheQualificationAwarded(new QuestionModel { Option = Options.England });

        result.Should().NotBeNull();

        var resultType = result as RedirectToActionResult;
        resultType.Should().NotBeNull();

        resultType!.ActionName.Should().Be("Get");
        resultType.ControllerName.Should().Be("QualificationDetails");
    }
}
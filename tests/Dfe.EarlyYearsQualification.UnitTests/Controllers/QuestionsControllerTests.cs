using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Dfe.EarlyYearsQualification.UnitTests.Controllers;

[TestClass]
public class QuestionsControllerTests
{

    private readonly ILogger<QuestionsController> _mockLogger = new NullLoggerFactory().CreateLogger<QuestionsController>();
    private Mock<IContentService> _mockContentService = new();
    private QuestionsController? _controller;

    [TestInitialize]
    public void BeforeEachTest()
    {
        _mockContentService = new Mock<IContentService>();
        _controller = new QuestionsController(_mockLogger, _mockContentService.Object);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsNoQuestionPage_RedirectsToErrorPage()
    {
        _mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded)).ReturnsAsync((QuestionPage)default!).Verifiable();
        var result = await _controller!.WhereWasTheQualificationAwarded();

        _mockContentService.VerifyAll();
        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("Error", resultType.ActionName);
        Assert.AreEqual("Home", resultType.ControllerName);
    }

    [TestMethod]
    public async Task WhereWasTheQualificationAwarded_ContentServiceReturnsQuestionPage_ReturnsAdvicePageModel()
    {
        var questionPage = new QuestionPage() { Question = "Test question", CtaButtonText = "Continue", Options = new List<Option> { new Option { Label = "Label", Value = "Value"} } };
        _mockContentService.Setup(x => x.GetQuestionPage(QuestionPages.WhereWasTheQualificationAwarded)).ReturnsAsync(questionPage);
        var result = await _controller!.WhereWasTheQualificationAwarded();

        Assert.IsNotNull(result);
        var resultType = result as ViewResult;
        Assert.IsNotNull(resultType);
        var model = resultType.Model as QuestionModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(questionPage.Question, model.Question);
        Assert.AreEqual(questionPage.CtaButtonText, model.CtaButtonText);
        Assert.AreEqual(1, model.Options.Count);
        Assert.AreEqual(questionPage.Options[0].Label, model.Options[0].Label);
        Assert.AreEqual(questionPage.Options[0].Value, model.Options[0].Value);
    }

    [TestMethod]
    public void Post_WhereWasTheQualificationAwarded_PassInOutsideUk_RedirectsToAdvicePage()
    {
        var result = _controller!.WhereWasTheQualificationAwarded(Options.OutsideOfTheUnitedKingdom);

        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("QualificationOutsideTheUnitedKingdom", resultType.ActionName);
        Assert.AreEqual("Advice", resultType.ControllerName);
    }

    [TestMethod]
    public void Post_WhereWasTheQualificationAwarded_PassInEngland_RedirectsToQualificationDetails()
    {
        var result = _controller!.WhereWasTheQualificationAwarded(Options.England);

        Assert.IsNotNull(result);
        var resultType = result as RedirectToActionResult;
        Assert.IsNotNull(resultType);
        Assert.AreEqual("Get", resultType.ActionName);
        Assert.AreEqual("QualificationDetails", resultType.ControllerName);
    }
}
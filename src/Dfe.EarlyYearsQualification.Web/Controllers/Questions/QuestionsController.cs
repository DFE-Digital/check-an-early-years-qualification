using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Services.Help;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

[Route("/questions")]
public partial class QuestionsController(
    ILogger<QuestionsController> logger,
    IQuestionService questionService)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("start-new")]
    [ResponseCache(NoStore = true)]
    public IActionResult StartNew()
    {
        questionService.ResetUserJourneyCookie();
        return RedirectToAction(nameof(this.AreYouCheckingYourOwnQualification));
    }
}
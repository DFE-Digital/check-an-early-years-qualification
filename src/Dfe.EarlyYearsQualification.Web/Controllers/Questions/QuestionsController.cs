using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

[Route("/questions")]
public partial class QuestionsController(
    ILogger<QuestionsController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IQualificationsRepository repository,
    IDateQuestionModelValidator questionModelValidator,
    IPlaceholderUpdater placeholderUpdater,
    IRadioQuestionMapper radioQuestionMapper,
    IDropdownQuestionMapper dropdownQuestionMapper,
    IPreCheckPageMapper preCheckPageMapper)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("start-new")]
    [ResponseCache(NoStore = true)]
    public IActionResult StartNew()
    {
        userJourneyCookieService.ResetUserJourneyCookie();
        return RedirectToAction(nameof(this.WhereWasTheQualificationAwarded));
    }

    private async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName,
                                                   string? selectedAnswer)
    {
        var questionPage = await contentService.GetRadioQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapRadioModel(new RadioQuestionModel(), questionPage, actionName, controllerName,
                                        selectedAnswer);
        
        return View("Radio", model);
    }

    private async Task<RadioQuestionModel> MapRadioModel(RadioQuestionModel model, RadioQuestionPage question,
                                                         string actionName,
                                                         string controllerName,
                                                         string? selectedAnswer)
    {
        return await radioQuestionMapper.Map(model, question, actionName, controllerName, selectedAnswer);
    }
}
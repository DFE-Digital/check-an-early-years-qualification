using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        return await questionService.GetRadioView(QuestionPages.WhereWasTheQualificationAwarded,
                                  nameof(this.WhereWasTheQualificationAwarded),
                                  Questions, questionService.GetWhereWasQualificationAwarded());
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded([FromForm] RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await questionService.GetRadioQuestionPageContent(QuestionPages.WhereWasTheQualificationAwarded);

            if (questionPage is not null)
            {
                model = await questionService.Map(model, questionPage, nameof(this.WhereWasTheQualificationAwarded),
                                            Questions, model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        return questionService.RedirectBasedOnWhereTheQualificationWasAwarded(model.Option);
    }
}
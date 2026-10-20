using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [RedirectIfDateMissing]
    [HttpGet("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification()
    {
        return await GetRadioView(QuestionPages.WhatLevelIsTheQualification, nameof(this.WhatLevelIsTheQualification),
                                  Questions, questionService.GetLevelOfQualification()?.ToString());
    }
    
    [RedirectIfDateMissing]
    [HttpPost("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification([FromForm] RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await questionService.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification);

            if (questionPage is not null)
            {
                model = await questionService.Map(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions,
                                            model.Option);

                model.ErrorSummaryModel = CreateErrorSummaryModel(model);
            }

            return View("Radio", model);
        }

        return questionService.RedirectBasedOnQualificationLevelSelected(model.Option);
    }

    /// <summary>
    ///     Saves any level the user has selected (without requiring one) before sending them to the
    ///     "Early years qualification levels" page, so it is pre-selected when they come back.
    /// </summary>
    [RedirectIfDateMissing]
    [HttpPost("what-level-is-the-qualification/qualification-levels")]
    public async Task<IActionResult> SaveLevelAndViewQualificationLevels([FromForm] RadioQuestionModel model)
    {
        var questionPage = await questionService.GetRadioQuestionPageContent(QuestionPages.WhatLevelIsTheQualification);

        var isValidOption = questionPage is not null
                            && questionPage.Options.OfType<Option>().Any(x => x.Value == model.Option);

        if (isValidOption)
        {
            questionService.SetLevelOfQualification(model.Option);
        }

        return RedirectToAction(nameof(AdviceController.EarlyYearsQualificationLevels), "Advice");
    }
}
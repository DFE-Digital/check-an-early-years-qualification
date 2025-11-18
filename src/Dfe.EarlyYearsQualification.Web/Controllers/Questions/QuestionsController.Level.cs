using Dfe.EarlyYearsQualification.Content.Constants;
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

                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        return questionService.RedirectBasedOnQualificationLevelSelected(model.Option);
    }
}
﻿using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("are-you-checking-your-own-qualification")]
    public async Task<IActionResult> AreYouCheckingYourOwnQualification()
    {
        return await GetRadioView(QuestionPages.AreYouCheckingYourOwnQualification, nameof(this.AreYouCheckingYourOwnQualification),
                                  Questions, userJourneyCookieService.GetIsUserCheckingTheirOwnQualification());
    }
    
    [HttpPost("are-you-checking-your-own-qualification")]
    public async Task<IActionResult> AreYouCheckingYourOwnQualification([FromForm] RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.AreYouCheckingYourOwnQualification);

            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.AreYouCheckingYourOwnQualification), Questions,
                                            model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetIsUserCheckingTheirOwnQualification(model.Option);

        return RedirectToAction(nameof(this.WhereWasTheQualificationAwarded));
    }
}
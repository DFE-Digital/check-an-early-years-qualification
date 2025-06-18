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
                                  Questions, userJourneyCookieService.GetLevelOfQualification()?.ToString());
    }
    
    [RedirectIfDateMissing]
    [HttpPost("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification([FromForm] RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);

            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions,
                                            model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetLevelOfQualification(model.Option);

        return model.Option switch
               {
                   "2" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                       RedirectToAction("QualificationsStartedBetweenSept2014AndAug2019", "Advice"),
                   "7" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                       RedirectToAction(nameof(AdviceController.Level7QualificationStartedBetweenSept2014AndAug2019),
                                        "Advice"),
                   "7" when userJourneyCookieService.WasStartedOnOrAfterSeptember2019() =>
                       RedirectToAction(nameof(AdviceController.Level7QualificationAfterAug2019), "Advice"),
                   _ => RedirectToAction(nameof(this.WhatIsTheAwardingOrganisation))
               };
    }
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        return await GetRadioView(QuestionPages.WhereWasTheQualificationAwarded,
                                  nameof(this.WhereWasTheQualificationAwarded),
                                  Questions, userJourneyCookieService.GetWhereWasQualificationAwarded());
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded([FromForm] RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);

            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhereWasTheQualificationAwarded),
                                            Questions, model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetWhereWasQualificationAwarded(model.Option);

        return model.Option switch
               {
                   QualificationAwardLocation.OutsideOfTheUnitedKingdom =>
                       RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice"),
                   QualificationAwardLocation.Scotland =>
                       RedirectToAction("QualificationsAchievedInScotland", "Advice"),
                   QualificationAwardLocation.Wales => RedirectToAction("QualificationsAchievedInWales", "Advice"),
                   QualificationAwardLocation.NorthernIreland =>
                       RedirectToAction("QualificationsAchievedInNorthernIreland", "Advice"),
                   _ => RedirectToAction(nameof(this.WhenWasTheQualificationStarted))
               };
    }
}
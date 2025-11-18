using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [RedirectIfDateMissing]
    [HttpGet("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation()
    {
        var questionPage = await questionService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var qualifications = await questionService.GetFilteredQualifications();

        var model = await questionService.MapDropdownModel(new DropdownQuestionModel(), questionPage, qualifications,
                                           nameof(this.WhatIsTheAwardingOrganisation),
                                           Questions,
                                           questionService.GetAwardingOrganisation(),
                                           questionService.GetAwardingOrganisationIsNotOnList());

        return View("Dropdown", model);
    }

    [RedirectIfDateMissing]
    [HttpPost("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation([FromForm] DropdownQuestionModel model)
    {
        if (!ModelState.IsValid || (string.IsNullOrEmpty(model.SelectedValue) && !model.NotInTheList))
        {
            var questionPage =
                await questionService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);

            if (questionPage is not null)
            {
                var qualifications = await questionService.GetFilteredQualifications();

                model = await questionService.MapDropdownModel(model, questionPage, qualifications,
                                               nameof(this.WhatIsTheAwardingOrganisation),
                                               Questions,
                                               model.SelectedValue,
                                               model.NotInTheList);
                model.HasErrors = true;
            }

            return View("Dropdown", model);
        }

        questionService.SetWhatIsTheAwardingOrganisationValuesInCookie(model);

        return RedirectToAction("Index", "CheckYourAnswers");
    }
}
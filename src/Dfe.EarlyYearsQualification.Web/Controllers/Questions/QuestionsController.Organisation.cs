using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Questions;

public partial class QuestionsController
{
    [RedirectIfDateMissing]
    [HttpGet("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation()
    {
        var questionPage = await contentService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var qualifications = await GetFilteredQualifications();

        var model = await MapDropdownModel(new DropdownQuestionModel(), questionPage, qualifications,
                                           nameof(this.WhatIsTheAwardingOrganisation),
                                           Questions,
                                           userJourneyCookieService.GetAwardingOrganisation(),
                                           userJourneyCookieService.GetAwardingOrganisationIsNotOnList());

        return View("Dropdown", model);
    }

    [RedirectIfDateMissing]
    [HttpPost("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation([FromForm] DropdownQuestionModel model)
    {
        if (!ModelState.IsValid || (string.IsNullOrEmpty(model.SelectedValue) && !model.NotInTheList))
        {
            var questionPage =
                await contentService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);

            if (questionPage is not null)
            {
                var qualifications = await GetFilteredQualifications();

                model = await MapDropdownModel(model, questionPage, qualifications,
                                               nameof(this.WhatIsTheAwardingOrganisation),
                                               Questions,
                                               model.SelectedValue,
                                               model.NotInTheList);
                model.HasErrors = true;
            }

            return View("Dropdown", model);
        }

        userJourneyCookieService.SetQualificationNameSearchCriteria(string.Empty);
        userJourneyCookieService.SetAwardingOrganisation(model.NotInTheList ? string.Empty : model.SelectedValue!);
        userJourneyCookieService.SetAwardingOrganisationNotOnList(model.NotInTheList);

        return RedirectToAction("Index", "CheckYourAnswers");
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        return await repository.Get(level, startDateMonth, startDateYear, null, null);
    }

    private async Task<DropdownQuestionModel> MapDropdownModel(DropdownQuestionModel model,
                                                               DropdownQuestionPage question,
                                                               List<Qualification> qualifications, string actionName,
                                                               string controllerName,
                                                               string? selectedAwardingOrganisation,
                                                               bool selectedNotOnTheList)
    {
        string[] awardingOrganisationExclusions =
            [AwardingOrganisations.AllHigherEducation, AwardingOrganisations.Various];

        var uniqueAwardingOrganisations
            = qualifications.Select(x => x.AwardingOrganisationTitle)
                            .Distinct()
                            .Where(x => !Array.Exists(awardingOrganisationExclusions, x.Contains))
                            .Order();

        var additionalInformationBodyHtml = await contentParser.ToHtml(question.AdditionalInformationBody);

        return DropdownQuestionMapper.Map(model, question, actionName, controllerName, uniqueAwardingOrganisations,
                                          additionalInformationBodyHtml, selectedAwardingOrganisation,
                                          selectedNotOnTheList);
    }
}
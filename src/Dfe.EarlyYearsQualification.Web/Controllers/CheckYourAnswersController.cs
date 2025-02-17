using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("questions/check-your-answers")]
[RedirectIfDateMissing]
public class CheckYourAnswersController(
    ILogger<CheckYourAnswersController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var whereWasQualificationAwardedQuestion =
            await contentService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);
        var whenWasTheQualificationStartedAndAwardedQuestion =
            await contentService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        var whatLevelIsTheQualificationQuestion =
            await contentService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);
        var whatIsTheAwardingOrganisationQuestion =
            await contentService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);
        var pageContent = await contentService.GetCheckYourAnswersPage();

        if (whereWasQualificationAwardedQuestion == null || whenWasTheQualificationStartedAndAwardedQuestion == null ||
            whatLevelIsTheQualificationQuestion == null || whatIsTheAwardingOrganisationQuestion == null ||
            pageContent == null)
        {
            logger.LogError("No content for the check your answers page");
            return RedirectToAction("Index", "Error");
        }

        var model = MapModel(pageContent, whereWasQualificationAwardedQuestion,
                             whenWasTheQualificationStartedAndAwardedQuestion,
                             whatLevelIsTheQualificationQuestion, whatIsTheAwardingOrganisationQuestion);

        return View(model);
    }

    private CheckYourAnswersPageModel MapModel(CheckYourAnswersPage pageContent,
                                               RadioQuestionPage whereWasQualificationAwardedQuestion,
                                               DatesQuestionPage whenWasTheQualificationStartedAndAwardedQuestion,
                                               RadioQuestionPage whatLevelIsTheQualificationQuestion,
                                               DropdownQuestionPage whatIsTheAwardingOrganisationQuestion)
    {
        var whereWasQualificationAwardedAnswer = userJourneyCookieService.GetWhereWasQualificationAwarded();
        var whenWasTheQualificationStartedAnswer = userJourneyCookieService.GetWhenWasQualificationStarted();
        // TODO: Update reference when new method available
        var whenWasTheQualificationAwardedAnswer = userJourneyCookieService.GetWhenWasQualificationStarted();
        var whatLevelIsTheQualificationAnswer = userJourneyCookieService.GetLevelOfQualification();
        var whatIsTheAwardingOrganisationAnswer = userJourneyCookieService.GetAwardingOrganisation();
        
        return CheckYourAnswersPageMapper.Map(pageContent,whereWasQualificationAwardedQuestion.Question,
                                       whenWasTheQualificationStartedAndAwardedQuestion.Question,
                                       whatLevelIsTheQualificationQuestion.Question,
                                       whatIsTheAwardingOrganisationQuestion.Question,
                                       whereWasQualificationAwardedAnswer,
                                       whenWasTheQualificationStartedAnswer,
                                       whenWasTheQualificationAwardedAnswer,
                                       whatLevelIsTheQualificationAnswer,
                                       whatIsTheAwardingOrganisationAnswer);
    }
}
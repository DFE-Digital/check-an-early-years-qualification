using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("confirm-qualification")]
[RedirectIfDateMissing]
public class ConfirmQualificationController(
    ILogger<ConfirmQualificationController> logger,
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IGovUkContentParser contentParser)
    : ServiceController
{
    [HttpGet]
    [Route("{qualificationId}")]
#pragma warning disable S6967
    // ...the model is a string, so no need to check ModelState.IsValid here
    public async Task<IActionResult> Index(string qualificationId)
#pragma warning restore S6967
    {
        if (string.IsNullOrEmpty(qualificationId))
        {
            return BadRequest();
        }

        var content = await contentService.GetConfirmQualificationPage();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await qualificationsRepository.GetById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        var model = await Map(content, qualification);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Confirm([FromForm] ConfirmQualificationPageModel model)
    {
        if (string.IsNullOrEmpty(model.QualificationId))
        {
            logger.LogError("No qualification id provided");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await qualificationsRepository.GetById(model.QualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = model.QualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        if (ModelState.IsValid)
        {
            userJourneyCookieService.SetUserSelectedQualificationFromList(YesOrNo.Yes);
            userJourneyCookieService.ClearAdditionalQuestionsAnswers();

            var hasAdditionalQuestions = qualification is
                                         {
                                             IsAutomaticallyApprovedAtLevel6: false,
                                             AdditionalRequirementQuestions.Count: > 0
                                         };

            switch (model.ConfirmQualificationAnswer)
            {
                case "yes":
                    userJourneyCookieService.SetSelectedQualificationName(qualification.QualificationName);

                    if (hasAdditionalQuestions)
                    {
                        return RedirectToAction("Index", "CheckAdditionalRequirements",
                           new
                           {
                               qualificationId =
                                   model.QualificationId,
                               questionIndex = 1
                           }
                       );
                    }

                    return RedirectToAction("Index", "QualificationDetails",
                        new
                        {
                            qualificationId = model.QualificationId
                        }
                    );
                default:
                    return RedirectToAction("Get", "QualificationSearch");
                }
            }

        var content = await contentService.GetConfirmQualificationPage();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }

        model = await Map(content, qualification);
        model.HasErrors = true;

        return View("Index", model);
    }

    private async Task<ConfirmQualificationPageModel> Map(ConfirmQualificationPage content, Qualification qualification)
    {
        var postHeadingContent = await contentParser.ToHtml(content.PostHeadingContent);
        var variousAwardingOrganisationsExplanation =
            await contentParser.ToHtml(content.VariousAwardingOrganisationsExplanation);

        return ConfirmQualificationPageMapper.Map(content, qualification, postHeadingContent,
                                                  variousAwardingOrganisationsExplanation);
    }
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("confirm-qualification")]
[RedirectIfDateMissing]
public class ConfirmQualificationController(
    ILogger<ConfirmQualificationController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IConfirmQualificationPageMapper confirmQualificationPageMapper,
    IQualificationSearchService qualificationSearchService)
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

        var filteredQualifications = await qualificationSearchService.GetFilteredQualifications();

        var qualification = filteredQualifications.SingleOrDefault(x => x.QualificationId.Equals(qualificationId, StringComparison.OrdinalIgnoreCase));

        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        var model = await confirmQualificationPageMapper.Map(content, qualification, filteredQualifications);

        // Used to prepopulate help form
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();
        enquiry.QualificationName = qualification.QualificationName;
        enquiry.AwardingOrganisation = SetHelpFormAwardingQualification(userJourneyCookieService.GetAwardingOrganisation(), qualification.AwardingOrganisationTitle);
        userJourneyCookieService.SetHelpFormEnquiry(enquiry);

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

        var filteredQualifications = await qualificationSearchService.GetFilteredQualifications();

        var qualification = filteredQualifications.SingleOrDefault(x => x.QualificationId == model.QualificationId);

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

        model = await confirmQualificationPageMapper.Map(content, qualification, filteredQualifications);
        model.HasErrors = true;

        return View("Index", model);
    }

    protected internal string SetHelpFormAwardingQualification(string? awardingOrgFromDropdown, string qualificationTitle)
    {
        // awardingOrgFromDropdown will be null if selected "Not in list"
        awardingOrgFromDropdown = awardingOrgFromDropdown ?? string.Empty;

        if (awardingOrgFromDropdown == string.Empty && qualificationTitle != AwardingOrganisations.Various)
        {
            return qualificationTitle;
        }

        return awardingOrgFromDropdown;
    }
}
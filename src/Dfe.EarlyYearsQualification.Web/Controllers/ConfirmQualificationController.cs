using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.ConfirmQualification;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("confirm-qualification")]
[RedirectIfDateMissing]
public class ConfirmQualificationController(
    ILogger<ConfirmQualificationController> logger,
    IConfirmQualificationService confirmQualificationService)
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

        var content = await confirmQualificationService.GetConfirmQualificationPageAsync();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await confirmQualificationService.GetQualificationById(qualificationId);

        if (qualification is null)
        {
            logger.LogError("Could not find details for qualification with ID: {QualificationId}", qualificationId);
            return RedirectToAction("Index", "Error");
        }

        var filteredQualifications = await confirmQualificationService.GetFilteredQualifications(qualification.QualificationName);
        var model = await confirmQualificationService.Map(content, qualification, filteredQualifications);
            
        // Used to prepopulate help form
        var enquiry = confirmQualificationService.GetHelpFormEnquiry();
        enquiry.QualificationName = qualification.QualificationName;
        enquiry.AwardingOrganisation = confirmQualificationService.SetHelpFormAwardingQualification(qualification.AwardingOrganisationTitle);
        confirmQualificationService.SetHelpFormEnquiry(enquiry);

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

        var qualification = await confirmQualificationService.GetQualificationById(model.QualificationId);

        if (qualification is null)
        {
            logger.LogError("Could not find details for qualification with ID: {QualificationId}", model.QualificationId);
            return RedirectToAction("Index", "Error");
        }

        if (ModelState.IsValid)
        {
            confirmQualificationService.ValidSubmitSetCookieValues();

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

        var content = await confirmQualificationService.GetConfirmQualificationPageAsync();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }
        
        var filteredQualifications = await confirmQualificationService.GetFilteredQualifications(qualification.QualificationName);
        model = await confirmQualificationService.Map(content, qualification, filteredQualifications);
        model.HasErrors = true;

        return View("Index", model);
    }
}
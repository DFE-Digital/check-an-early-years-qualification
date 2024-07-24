using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("confirm-qualification")]
public class ConfirmQualificationController(
    ILogger<ConfirmQualificationController> logger,
    IContentService contentService)
    : ServiceController
{
    [HttpGet]
    [Route("{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
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

        var qualification = await contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        var model = Map(content, qualification);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Confirm(ConfirmQualificationPageModel model)
    {
        if (string.IsNullOrEmpty(model.QualificationId))
        {
            logger.LogError("No qualification id provided");
            return RedirectToAction("Index", "Error");
        }
        
        var qualification = await contentService.GetQualificationById(model.QualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = model.QualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }
        
        if (ModelState.IsValid)
        {
            var hasAdditionalQuestions = qualification.AdditionalRequirementQuestions is not null &&
                                         qualification.AdditionalRequirementQuestions.Count > 0;
            return model.ConfirmQualificationAnswer == "yes" && hasAdditionalQuestions 
                       ? RedirectToAction("Index", "CheckAdditionalRequirements", 
                                          new { qualificationId = model.QualificationId }) 
                           : model.ConfirmQualificationAnswer == "yes"
                           ? RedirectToAction("Index", "QualificationDetails",
                                              new { qualificationId = model.QualificationId })
                           : RedirectToAction("Get", "QualificationDetails");
        }

        var content = await contentService.GetConfirmQualificationPage();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }

        model = Map(content, qualification);
        model.HasErrors = true;

        return View("Index", model);
    }

    private static ConfirmQualificationPageModel Map(ConfirmQualificationPage content, Qualification qualification)
    {
        return new ConfirmQualificationPageModel
               {
                   Heading = content.Heading,
                   Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
                   ErrorText = content.ErrorText,
                   LevelLabel = content.LevelLabel,
                   QualificationLabel = content.QualificationLabel,
                   RadioHeading = content.RadioHeading,
                   DateAddedLabel = content.DateAddedLabel,
                   AwardingOrganisationLabel = content.AwardingOrganisationLabel,
                   ErrorBannerHeading = content.ErrorBannerHeading,
                   ErrorBannerLink = content.ErrorBannerLink,
                   ButtonText = content.ButtonText,
                   HasErrors = false,
                   QualificationName = qualification.QualificationName,
                   QualificationLevel = qualification.QualificationLevel.ToString(),
                   QualificationId = qualification.QualificationId,
                   QualificationAwardingOrganisation = qualification.AwardingOrganisationTitle,
                   QualificationDateAdded = qualification.FromWhichYear!,
                   BackButton = content.BackButton
               };
    }
}
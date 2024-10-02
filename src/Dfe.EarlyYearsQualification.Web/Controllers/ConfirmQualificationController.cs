using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("confirm-qualification")]
[RedirectIfDateMissing]
public class ConfirmQualificationController(
    ILogger<ConfirmQualificationController> logger,
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

        var qualification = await contentService.GetQualificationById(qualificationId);
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
            userJourneyCookieService.SetUserSelectedQualificationFromList(YesOrNo.Yes);
            userJourneyCookieService.ClearAdditionalQuestionsAnswers();

            var hasAdditionalQuestions = qualification.AdditionalRequirementQuestions is not null &&
                                         qualification.AdditionalRequirementQuestions.Count > 0;

            return model.ConfirmQualificationAnswer switch
                   {
                       "yes" when hasAdditionalQuestions => RedirectToAction("Index",
                                                                             "CheckAdditionalRequirements",
                                                                             new
                                                                             {
                                                                                 qualificationId = model.QualificationId,
                                                                                 questionId = 1
                                                                             }),

                       "yes" => RedirectToAction("Index",
                                                 "QualificationDetails",
                                                 new { qualificationId = model.QualificationId }),

                       _ => RedirectToAction("Get", "QualificationDetails")
                   };
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
                   QualificationAwardingOrganisation = qualification.AwardingOrganisationTitle.Trim(),
                   QualificationDateAdded = qualification.FromWhichYear!,
                   BackButton = MapToNavigationLinkModel(content.BackButton),
                   PostHeadingContent = await contentParser.ToHtml(content.PostHeadingContent),
                   VariousAwardingOrganisationsExplanation = await contentParser.ToHtml(content.VariousAwardingOrganisationsExplanation)
               };
    }
}
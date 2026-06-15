using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.QualificationDetails;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
[RedirectIfDateMissing]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IQualificationDetailsService qualificationDetailsService
)
    : ServiceController
{
    [HttpGet("qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(qualificationId)) return BadRequest();
        if (!qualificationDetailsService.HasStartDate()) return RedirectToAction("Index", "Home");

        var qualification = await qualificationDetailsService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            logger.LogError("Could not find details for qualification with ID: {QualificationId}", qualificationId);
            return RedirectToAction("Index", "Error");
        }
       
        var (isFullAndRelevant, outcome) = await ValidateAdditionalQuestions(qualification);
        if (outcome == ValidateAdditionalRequirementOutcomes.RedirectToAdditionalRequirementQuestions)
        {
            return RedirectToAction("Index", "CheckAdditionalRequirements",
                                    new
                                    {
                                        qualification.QualificationId,
                                        questionIndex = 1
                                    }
                                   );
        }
        
        var content = await GetPageContent(qualification, isFullAndRelevant);

        if (content is null)
        {
            logger.LogError("No content for the qualification details page");
            return RedirectToAction("Index", "Error");
        }
        
        var model = await qualificationDetailsService.MapDetails(qualification, content);
        await qualificationDetailsService.SetRatioRequirements(qualification, model, content);
        await qualificationDetailsService.SetRatioText(model, content.Labels);
        if (model.RatioRequirements.IsNotFullAndRelevant)
        {
            qualificationDetailsService.SetQualificationResultFailureDetails(model, content.Labels);
        }
        else
        {
            qualificationDetailsService.SetQualificationResultSuccessDetails(model, content.Labels);
        }

        return View(model);
    }

    // [HttpGet("qualification-details/{qualificationId}/old")]
    // public async Task<IActionResult> IndexOld(string qualificationId)
    // {
    //     if (!ModelState.IsValid || string.IsNullOrEmpty(qualificationId)) return BadRequest();
    //     if (!qualificationDetailsService.HasStartDate()) return RedirectToAction("Index", "Home");
    //
    //     var qualification = await qualificationDetailsService.GetQualificationById(qualificationId);
    //     if (qualification is null)
    //     {
    //         logger.LogError("Could not find details for qualification with ID: {QualificationId}", qualificationId);
    //         return RedirectToAction("Index", "Error");
    //     }
    //
    //     var content = await GetPageContent(qualification);
    //
    //     if (content is null)
    //     {
    //         logger.LogError("No content for the qualification details page");
    //         return RedirectToAction("Index", "Error");
    //     }
    //
    //     var filteredQualifications =
    //         await qualificationDetailsService.GetFilteredQualifications(qualification.QualificationName);
    //
    //     var model = await qualificationDetailsService.MapDetails(qualification, content, filteredQualifications);
    //
    //     var validateAdditionalRequirementQuestions = await ValidateAdditionalQuestions(model, qualification);
    //
    //     
    //
    //     if (!validateAdditionalRequirementQuestions.isFullAndRelevant)
    //     {
    //         await qualificationDetailsService.SetDefaultCardContentForApprovedQualifications(qualification, model);
    //
    //         await qualificationDetailsService.QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
    //         qualificationDetailsService.SetQualificationResultFailureDetails(model, content.Labels);
    //         await qualificationDetailsService.QualificationMayBeEligibleForEbr(model, qualification);
    //         await qualificationDetailsService.QualificationMayBeEligibleForEyitt(model, qualification);
    //         await qualificationDetailsService.SetRatioText(model, content.Labels);
    //         return validateAdditionalRequirementQuestions.actionResult!;
    //     }
    //
    //     await qualificationDetailsService.CheckRatioRequirements(qualification, model);
    //     if (model.RatioRequirements.IsNotFullAndRelevant)
    //     {
    //         qualificationDetailsService.SetQualificationResultFailureDetails(model, content.Labels);
    //     }
    //     else
    //     {
    //         qualificationDetailsService.SetQualificationResultSuccessDetails(model, content.Labels);
    //     }
    //
    //     await qualificationDetailsService.QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
    //     await qualificationDetailsService.QualificationMayBeEligibleForEbr(model, qualification);
    //     await qualificationDetailsService.QualificationMayBeEligibleForEyitt(model, qualification);
    //     await qualificationDetailsService.SetRatioText(model, content.Labels);
    //
    //     return View(model);
    // }

    private async Task<QualificationDetailsPage?> GetPageContent(Qualification qualification, bool isFullAndRelevant)
    {
        var level = qualificationDetailsService.GetLevelOfQualification();
        var (startMonth, startYear) = qualificationDetailsService.GetWhenWasQualificationStarted();
        var isUserCheckingTheirOwnQualification = qualificationDetailsService.GetUserIsCheckingOwnQualification();

        if (level is not null && startMonth is not null && startYear is not null)
        {
            return await qualificationDetailsService.GetQualificationDetailsPage(
                        isUserCheckingTheirOwnQualification,
                        isFullAndRelevant,
                        // If the user selected not sure on the level page, use the qualification level instead
                        level.Value == 0 ? qualification.QualificationLevel : level.Value,
                        startMonth.Value,
                        startYear.Value,
                        qualification
                       );
        }

        return null;
    }

    private async Task<(bool isFullAndRelevant, ValidateAdditionalRequirementOutcomes outcome)> ValidateAdditionalQuestions(
        Qualification qualification)
    {
        var additionalRequirementQuestions =
            qualificationDetailsService.MapAdditionalRequirementAnswers(qualification.AdditionalRequirementQuestions);
        
        // If the qualification has no additional requirements then skip all checks and return.
        if (additionalRequirementQuestions == null) return (true, ValidateAdditionalRequirementOutcomes.Default);
        
        
        var details = new QualificationDetailsModel
                      {
                          AdditionalRequirementAnswers = additionalRequirementQuestions
                      };

        // If qualification contains the QTS question, check the answers
        if (qualificationDetailsService.QualificationContainsQtsQuestion(qualification))
            return await CheckAnswersWhereQtsAnswered(details, qualification);

        // If there is a mismatch between the questions answered, then clear the answers and navigate back to the additional requirements check page
        if (qualificationDetailsService.DoAdditionalAnswersMatchQuestions(details))
        {
            return (false, ValidateAdditionalRequirementOutcomes.RedirectToAdditionalRequirementQuestions);
        }

        // If there are not any answers to the questions that are not full and relevant we can continue back to check the ratios.
        if (!qualificationDetailsService.AnswersIndicateNotFullAndRelevant(details.AdditionalRequirementAnswers))
            return (true, ValidateAdditionalRequirementOutcomes.Default);

        // At this point, there will be at least one question answered in a non full and relevant way.
        // we mark the ratios as not full and relevant and return.
        details.RatioRequirements = qualificationDetailsService.MarkAsNotFullAndRelevant(details.RatioRequirements);
        return (false, ValidateAdditionalRequirementOutcomes.Default);
    }

    private async Task<(bool isFullAndRelevant, ValidateAdditionalRequirementOutcomes outcome)> CheckAnswersWhereQtsAnswered(
        QualificationDetailsModel details, Qualification qualification)
    {
        var qtsQuestion =
            qualification.AdditionalRequirementQuestions!.First(x => x.Sys.Id == AdditionalRequirementQuestions
                                                                         .QtsQuestion);

        if (qualificationDetailsService.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification,
             details.AdditionalRequirementAnswers))
        {
            // Remove the additional requirements that they didn't answer following the bypass.
            details.AdditionalRequirementAnswers!.RemoveAll(x => x.Question != qtsQuestion.Question);
            return (true, ValidateAdditionalRequirementOutcomes.Default);
        }

        var remainingAnswersIndicateFullAndRelevant =
            qualificationDetailsService.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion);
        if (remainingAnswersIndicateFullAndRelevant.isFullAndRelevant) return (true, ValidateAdditionalRequirementOutcomes.Default);

        details = await qualificationDetailsService.CheckLevel6Requirements(qualification, details);

        return (false, ValidateAdditionalRequirementOutcomes.Default);
    }
}
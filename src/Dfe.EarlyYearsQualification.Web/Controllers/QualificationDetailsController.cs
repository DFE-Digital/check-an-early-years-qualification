using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Attributes;
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

        var detailsPageContent = await qualificationDetailsService.GetDetailsPage();
        if (detailsPageContent is null)
        {
            logger.LogError("No content for the qualification details page");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await qualificationDetailsService.GetQualification(qualificationId);
        if (qualification is null)
        {
            logger.LogError("Could not find details for qualification with ID: {QualificationId}", qualificationId.Replace(Environment.NewLine, ""));
            return RedirectToAction("Index", "Error");
        }

        var model = await qualificationDetailsService.MapDetails(qualification, detailsPageContent);

        var validateAdditionalRequirementQuestions = await ValidateAdditionalQuestions(model, qualification);

        if (!validateAdditionalRequirementQuestions.isValid)
        {
            await qualificationDetailsService.QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
            await qualificationDetailsService.SetRatioText(model, detailsPageContent);
            return validateAdditionalRequirementQuestions.actionResult!;
        }

        await qualificationDetailsService.CheckRatioRequirements(qualification, model);
        await qualificationDetailsService.QualificationLevel3OrAboveMightBeRelevantAtLevel2(model, qualification);
        await qualificationDetailsService.SetRatioText(model, detailsPageContent);

        return View(model);
    }

    private async Task<(bool isValid, IActionResult? actionResult)> ValidateAdditionalQuestions(QualificationDetailsModel details, Qualification qualification)
    {
        // If the qualification has no additional requirements then skip all checks and return.
        if (details.AdditionalRequirementAnswers == null) return (true, null);

        // If qualification contains the QTS question, check the answers
        if (qualificationDetailsService.QualificationContainsQtsQuestion(qualification)) return await CheckAnswersWhereQtsAnswered(details, qualification);

        // If there is a mismatch between the questions answered, then clear the answers and navigate back to the additional requirements check page
        if (qualificationDetailsService.DoAdditionalAnswersMatchQuestions(details))
        {
            return (false,
                    RedirectToAction("Index", "CheckAdditionalRequirements",
                                     new
                                     {
                                         details.QualificationId,
                                         questionIndex = 1
                                     }
                                    )
                   );
        }

        // If there are not any answers to the questions that are not full and relevant we can continue back to check the ratios.
        if (!qualificationDetailsService.AnswersIndicateNotFullAndRelevant(details.AdditionalRequirementAnswers)) return (true, null);

        // At this point, there will be at least one question answered in a non full and relevant way.
        // we mark the ratios as not full and relevant and return.
        details.RatioRequirements = qualificationDetailsService.MarkAsNotFullAndRelevant(details.RatioRequirements);
        return (false, View(details));
    }

    private async Task<(bool isValid, IActionResult? actionResult)> CheckAnswersWhereQtsAnswered(QualificationDetailsModel details, Qualification qualification)
    {
        var qtsQuestion = qualification.AdditionalRequirementQuestions!.First(x => x.Sys.Id == AdditionalRequirementQuestions.QtsQuestion);

        if (qualificationDetailsService.UserAnswerMatchesQtsQuestionAnswerToBeFullAndRelevant(qualification, details.AdditionalRequirementAnswers))
        {
            // Remove the additional requirements that they didn't answer following the bypass.
            details.AdditionalRequirementAnswers!.RemoveAll(x => x.Question != qtsQuestion.Question);
            return (true, null);
        }

        var remainingAnswersIndicateFullAndRelevant = qualificationDetailsService.RemainingAnswersIndicateFullAndRelevant(details, qtsQuestion);
        if (remainingAnswersIndicateFullAndRelevant.isFullAndRelevant) return (true, null);

        details = await qualificationDetailsService.CheckLevel6Requirements(qualification, details);

        return (false, View(details));
    }
}
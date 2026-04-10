using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
{
    [HttpGet("qualification-details")]
    public async Task<IActionResult> QualificationDetails()
    {
        var content = await helpService.GetHelpQualificationDetailsPageAsync();

        if (content is null)
        {
            logger.LogError("'Help qualification details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = new QualificationDetailsPageViewModel();

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        // set any previously entered qualification details from cookie
        helpService.SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel);

        viewModel = helpService.MapHelpQualificationDetailsPageContentToViewModel(viewModel, content, null, ModelState);

        return View("QualificationDetails", viewModel);
    }

    [HttpPost("qualification-details")]
    public async Task<IActionResult> QualificationDetails([FromForm] QualificationDetailsPageViewModel model)
    {
        var content = await helpService.GetHelpQualificationDetailsPageAsync();

        if (content is null)
        {
            logger.LogError("'Help qualification details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var datesValidationResult = helpService.ValidateDates(model.QuestionModel, content);

        if (datesValidationResult.StartedValidationResult is null)
        {
            // optional date fields not provided so remove the required validation
            ModelState.Remove("QuestionModel.StartedQuestion.SelectedMonth");
            ModelState.Remove("QuestionModel.StartedQuestion.SelectedYear");
        }

        var hasInvalidDates = helpService.HasInvalidDates(datesValidationResult);

        if (!ModelState.IsValid || hasInvalidDates)
        {
            model.HasQualificationNameError = ModelState.Keys.Any(_ => ModelState["QualificationName"]?.Errors.Count > 0);
            model.HasAwardingOrganisationError = ModelState.Keys.Any(_ => ModelState["AwardingOrganisation"]?.Errors.Count > 0);
            model.QuestionModel.HasErrors = hasInvalidDates;

            helpService.MapHelpQualificationDetailsPageContentToViewModel(model, content, datesValidationResult, ModelState);

            return View("QualificationDetails", model);
        }

        // valid submit 
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        helpService.SetHelpQualificationDetailsInCookie(enquiry, model);

        return RedirectToAction(nameof(ProvideDetails));
    }
}

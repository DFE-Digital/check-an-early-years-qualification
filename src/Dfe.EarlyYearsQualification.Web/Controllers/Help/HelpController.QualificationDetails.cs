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

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var viewModel = helpService.MapHelpQualificationDetailsPageContentToViewModel(new QualificationDetailsPageViewModel(), content);

        helpService.SetAnyPreviouslyEnteredQualificationDetailsFromCookie(viewModel, content);

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

        model = helpService.MapHelpQualificationDetailsPageContentToViewModel(model, content);

        helpService.AddQualificationDetailsValidationErrors(model, content, ModelState);

        if (!ModelState.IsValid || model.Errors.Any())
        {
            return View("QualificationDetails", model);
        }

        helpService.SetHelpQualificationDetailsInCookie(model);

        return RedirectToAction(nameof(ProvideDetails));
    }
}

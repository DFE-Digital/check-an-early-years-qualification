using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
{
    [HttpGet("confirmation")]
    public async Task<IActionResult> Confirmation()
    {
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var content = await helpService.GetHelpConfirmationPage();

        if (content is null)
        {
            logger.LogError("'Help confirmation page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = await helpService.MapConfirmationPageContentToViewModelAsync(content);

        return View("Confirmation", viewModel);
    }
}

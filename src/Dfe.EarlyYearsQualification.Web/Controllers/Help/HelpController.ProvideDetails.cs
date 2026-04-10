using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
{
    [HttpGet("provide-details")]
    public async Task<IActionResult> ProvideDetails()
    {
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var content = await helpService.GetHelpProvideDetailsPage();

        if (content is null)
        {
            logger.LogError("'Help provide details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = helpService.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring);

        viewModel.ProvideAdditionalInformation = enquiry.AdditionalInformation;

        return View("ProvideDetails", viewModel);
    }

    [HttpPost("provide-details")]
    public async Task<IActionResult> ProvideDetails([FromForm] ProvideDetailsPageViewModel viewModel)
    {
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        if (!ModelState.IsValid)
        {
            var content = await helpService.GetHelpProvideDetailsPage();

            if (content is null)
            {
                logger.LogError("'Help provide details page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            viewModel = helpService.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring);

            viewModel.HasAdditionalInformationError = ModelState.Keys.Any(_ => ModelState["ProvideAdditionalInformation"]?.Errors.Count > 0);

            return View("ProvideDetails", viewModel);
        }

        // valid submit
        enquiry.AdditionalInformation = viewModel.ProvideAdditionalInformation;

        helpService.SetHelpFormEnquiry(enquiry);

        return RedirectToAction(nameof(EmailAddress));
    }
}

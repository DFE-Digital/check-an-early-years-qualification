using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
{
    [HttpGet("email-address")]
    public async Task<IActionResult> EmailAddress()
    {
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring) || string.IsNullOrEmpty(enquiry.AdditionalInformation))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var content = await helpService.GetHelpEmailAddressPage();

        if (content is null)
        {
            logger.LogError("'Help email address page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = helpService.MapEmailAddressPageContentToViewModel(content);

        return View("EmailAddress", viewModel);
    }

    [HttpPost("email-address")]
    public async Task<IActionResult> EmailAddress([FromForm] EmailAddressPageViewModel model)
    {
        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring) || string.IsNullOrEmpty(enquiry.AdditionalInformation))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        if (!ModelState.IsValid)
        {
            var content = await helpService.GetHelpEmailAddressPage();

            if (content is null)
            {
                logger.LogError("'Help email address page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            var viewModel = helpService.MapEmailAddressPageContentToViewModel(content);

            viewModel.HasEmailAddressError = string.IsNullOrEmpty(model.EmailAddress) || ModelState.Keys.Any(_ => ModelState["EmailAddress"]?.Errors.Count > 0);
            viewModel.EmailAddressErrorMessage = string.IsNullOrEmpty(model.EmailAddress)
                                            ? content.NoEmailAddressEnteredErrorMessage
                                            : content.InvalidEmailAddressErrorMessage;

            return View("EmailAddress", viewModel);
        }

        // send help form email
        helpService.SendHelpPageNotification(
            new HelpPageNotification(model.EmailAddress, enquiry)
        );

        // clear data collected from help form on successful submit except for enquiry reason which is needed to determine which confirmation page to show
        helpService.SetHelpFormEnquiry(new()
        {
            ReasonForEnquiring = enquiry.ReasonForEnquiring
        });

        return RedirectToAction(nameof(Confirmation));
    }
}

using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/help")]
public class HelpController(
    ILogger<HelpController> logger,
    Services.Help.IHelpService helpService
    )
    : ServiceController
{

    [HttpGet("get-help")]
    public async Task<IActionResult> GetHelp() 
    {
        var content = await helpService.GetGetHelpPageAsync();

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = await helpService.MapGetHelpPageContentToViewModelAsync(content);
        viewModel.SelectedOption = helpService.GetSelectedOption();

        return View("GetHelp", viewModel);
    } 

    [HttpPost("get-help")]
    public async Task<IActionResult> GetHelp([FromForm] GetHelpPageViewModel model)
    {
        var content = await helpService.GetGetHelpPageAsync();

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var submittedValueIsValid = helpService.SelectedOptionIsValid(content, model);

        if (!ModelState.IsValid || !submittedValueIsValid)
        {
            var viewModel = await helpService.MapGetHelpPageContentToViewModelAsync(content);
            viewModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0) || !submittedValueIsValid;

            return View("GetHelp", viewModel);
        }
        
        return helpService.SetHelpFormEnquiryReason(model);
    }

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

    [HttpGet("provide-details")]
    public async Task<IActionResult> ProvideDetails()
    {
        var content = await helpService.GetHelpProvideDetailsPage();

        if (content is null)
        {
            logger.LogError("'Help provide details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
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

    [HttpGet("email-address")]
    public async Task<IActionResult> EmailAddress()
    {
        var content = await helpService.GetHelpEmailAddressPage();

        if (content is null)
        {
            logger.LogError("'Help email address page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring) || string.IsNullOrEmpty(enquiry.AdditionalInformation))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var viewModel = helpService.MapEmailAddressPageContentToViewModel(content);

        return View("EmailAddress", viewModel);
    }

    [HttpPost("email-address")]
    public async Task<IActionResult> EmailAddress([FromForm] EmailAddressPageViewModel model)
    {
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

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring) || string.IsNullOrEmpty(enquiry.AdditionalInformation))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        // send help form email
        helpService.SendHelpPageNotification(
            new HelpPageNotification(model.EmailAddress, enquiry)
        );

        // clear data collected from help form on successful submit
        helpService.SetHelpFormEnquiry(new());

        return RedirectToAction(nameof(Confirmation));
    }

    [HttpGet("confirmation")]
    public async Task<IActionResult> Confirmation()
    {
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
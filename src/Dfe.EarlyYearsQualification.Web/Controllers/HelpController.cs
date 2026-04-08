using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;
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
        var content = await helpService.GetRadioQuestionHelpPageAsync(RadioQuestionHelpPages.GetHelp);

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = await helpService.MapRadioQuestionHelpPageContentToViewModelAsync(content);
        viewModel.SelectedOption = helpService.GetWhyAreYouContactingUsSelectedOption();
        viewModel.ActionName = nameof(GetHelp);
        viewModel.FormId = "get-help-enquiry-form";

        return View("RadioQuestion", viewModel);
    } 

    [HttpPost("get-help")]
    public async Task<IActionResult> GetHelp([FromForm] RadioQuestionHelpPageViewModel model)
    {
        var content = await helpService.GetRadioQuestionHelpPageAsync(RadioQuestionHelpPages.GetHelp);

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var submittedValueIsValid = helpService.SelectedOptionIsValid(content.Options, model.SelectedOption);

        if (!ModelState.IsValid || !submittedValueIsValid)
        {
            var viewModel = await helpService.MapRadioQuestionHelpPageContentToViewModelAsync(content);
            viewModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0) || !submittedValueIsValid;
            viewModel.ActionName = nameof(GetHelp);
            viewModel.FormId = "get-help-enquiry-form";

            return View("RadioQuestion", viewModel);
        }

        helpService.SetHelpFormEnquiryReason(model.SelectedOption);

        switch (model.SelectedOption)
        {
            case nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript):
                return RedirectToAction(nameof(INeedACopyOfTheQualificationCertificateOrTranscript));
            case nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs):
                return RedirectToAction(nameof(IDoNotKnowWhatLevelTheQualificationIs));
            case nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol):
                return RedirectToAction(nameof(IWantToCheckWhetherACourseIsApprovedBeforeIEnrol));
            case nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification):
                return RedirectToAction(nameof(ProceedWithQualificationQuery));
            case nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService):
                return RedirectToAction(nameof(ProvideDetails));
            default:
                logger.LogError("Unexpected enquiry option");
                return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet("I-need-a-copy-of-the-qualification-certificate-or-transcript")]
    public async Task<IActionResult> INeedACopyOfTheQualificationCertificateOrTranscript()
    {
        return await GetView(StaticPages.HowToGetACopyOfTheCertificateOrTranscript);
    }

    [HttpGet("I-do-not-know-what-level-the-qualification-is")]
    public async Task<IActionResult> IDoNotKnowWhatLevelTheQualificationIs()
    {
        return await GetView(StaticPages.HowToFindTheLevelOfAQualification);
    }

    [HttpGet("I-want-to-check-whether-a-course-is-approved-before-I-enrol")]
    public async Task<IActionResult> IWantToCheckWhetherACourseIsApprovedBeforeIEnrol()
    {
        return await GetView(StaticPages.HowToFindASuitableCourse);
    }

    [HttpGet("proceed-with-qualification-query")]
    public async Task<IActionResult> ProceedWithQualificationQuery()
    {
        var content = await helpService.GetRadioQuestionHelpPageAsync(RadioQuestionHelpPages.ProceedWithQualificationQuery);

        if (content is null)
        {
            logger.LogError("'Proceed with qualification query page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var enquiry = helpService.GetHelpFormEnquiry();

        if (string.IsNullOrEmpty(enquiry.ReasonForEnquiring))
        {
            logger.LogError("Help form enquiry reason is empty");
            return RedirectToAction("GetHelp", "Help");
        }

        var viewModel = await helpService.MapRadioQuestionHelpPageContentToViewModelAsync(content);
        viewModel.SelectedOption = helpService.GetWhatDoYouWantToDoNextSelectedOption();
        viewModel.ActionName = nameof(ProceedWithQualificationQuery);
        viewModel.FormId = "proceed-with-qualification-enquiry-form";

        return View("RadioQuestion", viewModel);
    }

    [HttpPost("proceed-with-qualification-query")]
    public async Task<IActionResult> ProceedWithQualificationQuery([FromForm] RadioQuestionHelpPageViewModel model)
    {
        var content = await helpService.GetRadioQuestionHelpPageAsync(RadioQuestionHelpPages.ProceedWithQualificationQuery);

        if (content is null)
        {
            logger.LogError("'Proceed with qualification query page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var submittedValueIsValid = helpService.SelectedOptionIsValid(content.Options, model.SelectedOption);

        if (!ModelState.IsValid || !submittedValueIsValid)
        {
            var viewModel = await helpService.MapRadioQuestionHelpPageContentToViewModelAsync(content);
            viewModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0) || !submittedValueIsValid;
            viewModel.ActionName = nameof(ProceedWithQualificationQuery);
            viewModel.FormId = "proceed-with-qualification-enquiry-form";

            return View("RadioQuestion", viewModel);
        }

        switch (model.SelectedOption)
        {
            case nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService):
                return RedirectToAction(nameof(HomeController.Index), "Home");
            case nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam):
                var enquiry = helpService.GetHelpFormEnquiry();
                enquiry.WhatDoYouWantToDoNext = HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam;
                helpService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(QualificationDetails));
            default:
                logger.LogError("Unexpected enquiry option");
                return RedirectToAction("Index", "Error");
        }
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

    private async Task<IActionResult> GetView(string staticPageId)
    {
        var staticPage = await helpService.GetStaticPage(staticPageId);
        if (staticPage is null)
        {
            logger.LogError("No content for the static page");
            return RedirectToAction("Index", "Error");
        }

        var model = await helpService.MapStaticPage(staticPage);

        return View("../Static/Static", model);
    }
}
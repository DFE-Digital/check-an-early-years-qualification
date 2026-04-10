using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
{
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
}

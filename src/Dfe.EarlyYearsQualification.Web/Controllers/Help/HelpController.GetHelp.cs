using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers.Help;

public partial class HelpController
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
}
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/help")]
public class HelpController(
    ILogger<HelpController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IDateQuestionModelValidator questionModelValidator,
    IPlaceholderUpdater placeholderUpdater
    )
    : ServiceController
{

    [HttpGet("get-help")]
    public async Task<IActionResult> GetHelp() 
    {
        var content = await contentService.GetGetHelpPage();

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = await HelpControllerPageMapper.MapGetHelpPageContentToViewModelAsync(content, contentParser);

        return View("GetHelp", viewModel);
    } 

    [HttpPost("get-help")]
    public async Task<IActionResult> GetHelp([FromForm] GetHelpPageViewModel model)
    {
        var content = await contentService.GetGetHelpPage();

        if (content is null)
        {
            logger.LogError("'Get help page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var submittedValueIsValid = content.EnquiryReasons.Select(x => x.Value).Contains(model.SelectedOption);

        if (!ModelState.IsValid || !submittedValueIsValid)
        {
            var viewModel = await HelpControllerPageMapper.MapGetHelpPageContentToViewModelAsync(content, contentParser);
            viewModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0) || !submittedValueIsValid;

            return View("GetHelp", viewModel);
        }

        // valid submit
        switch (model.SelectedOption)
        {
            case "QuestionAboutAQualification":
                userJourneyCookieService.SetHelpFormEnquiry(
                    new HelpFormEnquiry()
                    {
                        ReasonForEnquiring = "Question about a qualification",
                    }
                );
                return RedirectToAction(nameof(QualificationDetails));
            case "IssueWithTheService":
                userJourneyCookieService.SetHelpFormEnquiry(
                    new HelpFormEnquiry()
                    {
                        ReasonForEnquiring = "Issue with the service",
                    }
                );
                return RedirectToAction(nameof(ProvideDetails));
            default:
                logger.LogError("Unexpected enquiry option");
                return RedirectToAction("Index", "Error");
        }
    }

    [HttpGet("qualification-details")]
    public async Task<IActionResult> QualificationDetails()
    {
        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            logger.LogError("'Help qualification details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = new QualificationDetailsPageViewModel();

        // set any previously entered qualification details from cookie
        viewModel.AwardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        viewModel.QualificationName = userJourneyCookieService.GetSelectedQualificationName();
        var qualificationStart = userJourneyCookieService.GetWhenWasQualificationStarted();
        var qualificationAwarded = userJourneyCookieService.GetWhenWasQualificationAwarded();

        viewModel.QuestionModel.StartedQuestion = new DateQuestionModel()
        {
            SelectedMonth = qualificationStart.startMonth,
            SelectedYear = qualificationStart.startYear
        };
        viewModel.QuestionModel.AwardedQuestion = new DateQuestionModel()
        {
            SelectedMonth = qualificationAwarded.startMonth,
            SelectedYear = qualificationAwarded.startYear
        };

        viewModel = HelpControllerPageMapper.MapQualificationDetailsContentToViewModel(viewModel, content, null, ModelState, placeholderUpdater);

        return View("QualificationDetails", viewModel);
    }

    [HttpPost("qualification-details")]
    public async Task<IActionResult> QualificationDetails([FromForm] QualificationDetailsPageViewModel model)
    {
        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            logger.LogError("'Help qualification details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var datesValidationResult = questionModelValidator.IsValid(model.QuestionModel, content);

        if (datesValidationResult.StartedValidationResult is null)
        {
            // optional date fields not provided so remove the required validation
            ModelState.Remove("QuestionModel.StartedQuestion.SelectedMonth");
            ModelState.Remove("QuestionModel.StartedQuestion.SelectedYear");
        }

        var hasInvalidDates = !datesValidationResult.AwardedValidationResult.MonthValid || !datesValidationResult.AwardedValidationResult.YearValid ||
            (datesValidationResult.StartedValidationResult is not null && (!datesValidationResult.StartedValidationResult.MonthValid || !datesValidationResult.StartedValidationResult.YearValid));

        if (!ModelState.IsValid || hasInvalidDates)
        {
            model.HasQualificationNameError = ModelState.Keys.Any(_ => ModelState["QualificationName"]?.Errors.Count > 0);
            model.HasAwardingOrganisationError = ModelState.Keys.Any(_ => ModelState["AwardingOrganisation"]?.Errors.Count > 0);

            model.QuestionModel.HasErrors = hasInvalidDates;

            model = HelpControllerPageMapper.MapQualificationDetailsContentToViewModel(model, content, datesValidationResult, ModelState, placeholderUpdater);

            return View("QualificationDetails", model);
        }

        // valid submit 
        var helpCookie = userJourneyCookieService.GetHelpFormEnquiry();

        helpCookie.QualificationName = model.QualificationName;
        if(model.QuestionModel.StartedQuestion is not null)
        {
            helpCookie.QualificationStartDate = $"{model.QuestionModel.StartedQuestion?.SelectedMonth}/{model.QuestionModel.StartedQuestion?.SelectedYear}";
        }
        helpCookie.QualificationAwardedDate = $"{model.QuestionModel.AwardedQuestion.SelectedMonth}/{model.QuestionModel.AwardedQuestion.SelectedYear}";
        helpCookie.AwardingOrganisation = model.AwardingOrganisation;

        userJourneyCookieService.SetHelpFormEnquiry(helpCookie);

        return RedirectToAction(nameof(ProvideDetails));
    }

    [HttpGet("provide-details")]
    public async Task<IActionResult> ProvideDetails()
    {
        var content = await contentService.GetHelpProvideDetailsPage();

        if (content is null)
        {
            logger.LogError("'Help provide details page' content could not be found");
            return RedirectToAction("Index", "Error");
        }
  
        var viewModel = HelpControllerPageMapper.MapProvideDetailsPageContentToViewModel(content, ModelState, userJourneyCookieService.GetHelpFormEnquiry().ReasonForEnquiring);

        return View("ProvideDetails", viewModel);
    }

    [HttpPost("provide-details")]
    public async Task<IActionResult> ProvideDetails([FromForm] ProvideDetailsPageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var content = await contentService.GetHelpProvideDetailsPage();

            if (content is null)
            {
                logger.LogError("'Help provide details page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            viewModel = HelpControllerPageMapper.MapProvideDetailsPageContentToViewModel(content, ModelState, userJourneyCookieService.GetHelpFormEnquiry().ReasonForEnquiring);

            return View("ProvideDetails", viewModel);
        }

        // valid submit
        var helpCookie = userJourneyCookieService.GetHelpFormEnquiry();

        helpCookie.AdditionalInformation = viewModel.ProvideAdditionalInformation;

        userJourneyCookieService.SetHelpFormEnquiry(helpCookie);

        return RedirectToAction(nameof(EmailAddress));
    }

    [HttpGet("email-address")]
    public async Task<IActionResult> EmailAddress()
    {
        var content = await contentService.GetHelpEmailAddressPage();

        if (content is null)
        {
            logger.LogError("'Help email address page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = HelpControllerPageMapper.MapEmailAddressPageContentToViewModel(content, ModelState);

        return View("EmailAddress", viewModel);
    }

    [HttpPost("email-address")]
    public async Task<IActionResult> EmailAddress([FromForm] EmailAddressPageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var content = await contentService.GetHelpEmailAddressPage();

            if (content is null)
            {
                logger.LogError("'Help email address page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            viewModel = HelpControllerPageMapper.MapEmailAddressPageContentToViewModel(content, ModelState);

            return View("EmailAddress", viewModel);
        }

        // send help form email
        notificationService.SendHelpPageNotification(
            new HelpPageNotification(viewModel.EmailAddress, userJourneyCookieService.GetHelpFormEnquiry())
        );

        // clear data collected from help form on successful submit
        userJourneyCookieService.SetHelpFormEnquiry(new());

        return RedirectToAction(nameof(Confirmation));
    }

    [HttpGet("confirmation")]
    public async Task<IActionResult> Confirmation()
    {
        var content = await contentService.GetHelpConfirmationPage();

        if (content is null)
        {
            logger.LogError("'Help confirmation page' content could not be found");
            return RedirectToAction("Index", "Error");
        }

        var viewModel = await HelpControllerPageMapper.MapConfirmationPageContentToViewModelAsync(content, contentParser);

        return View("Confirmation", viewModel);
    }
}
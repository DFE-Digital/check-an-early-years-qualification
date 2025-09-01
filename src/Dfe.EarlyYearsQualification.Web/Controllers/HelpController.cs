using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/help")]
public class HelpController(
    ILogger<HelpController> logger, // todo check if should log or throw exception
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
    public async Task<IActionResult> GetHelpAsync() 
    {
        var content = await contentService.GetGetHelpPage();

        if (content is null)
        {
            throw new Exception("'Get help page' content could not be found");
        }

        var viewModel = await HelpControllerPageMapper.MapGetHelpPageContentToViewModelAsync(content, contentParser, ModelState);

        return View("GetHelp", viewModel);
    } 

    [HttpPost("get-help")]
    public async Task<IActionResult> Help([FromForm] GetHelpPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var content = await contentService.GetGetHelpPage();

            if (content is null)
            {
                throw new Exception("'Get help page' content could not be found");
            }

            var viewModel = await HelpControllerPageMapper.MapGetHelpPageContentToViewModelAsync(content, contentParser, ModelState);

            return View("GetHelp", viewModel);
        }

        // valid submit
        switch (model.SelectedOption)
        {
            case "QuestionAboutAQualification":
                userJourneyCookieService.SetHelpFormEnquiry(
                    new()
                    {
                        ReasonForEnquiring = "Question about a qualification",
                    }
                );
                return RedirectToAction("QualificationDetails");
            case "IssueWithTheService":
                userJourneyCookieService.SetHelpFormEnquiry(
                    new()
                    {
                        ReasonForEnquiring = "Issue with the service",
                    }
                );
                return RedirectToAction("ProvideDetails");
            default:
                throw new InvalidOperationException("Unexpected enquiry option"); //todo check this should we redirect home if someone tampers with html?
        }
    }

    [HttpGet("qualification-details")]
    public async Task<IActionResult> QualificationDetailsAsync()
    {
        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help qualification details page' content could not be found");
        }

        var viewModel = HelpControllerPageMapper.MapQualificationDetailsContentToViewModel(content, ModelState, new());

        // set any previously entered qualification details from cookie
        viewModel.AwardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        viewModel.QualificationName = userJourneyCookieService.GetSelectedQualificationName();

        var qualificationStart = userJourneyCookieService.GetWhenWasQualificationStarted();
        viewModel.StartDateSelectedMonth = qualificationStart.startMonth;
        viewModel.StartDateSelectedYear = qualificationStart.startYear;

        var qualificationAwarded = userJourneyCookieService.GetWhenWasQualificationAwarded();
        viewModel.QualificationAwardedDate.SelectedMonth = qualificationAwarded.startMonth;
        viewModel.QualificationAwardedDate.SelectedYear = qualificationAwarded.startYear;

        return View("QualificationDetails", viewModel);
    }

    [HttpPost("qualification-details")]
    public async Task<IActionResult> QualificationDetailsAsync([FromForm] QualificationDetailsPageViewModel viewModel)
    {
        var datesAreInvalid = false;

        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help qualification details page' content could not be found");
        }

        // Map details to date question so they can be validated
        var startedQuestion = new DateQuestionModel()
        {
            SelectedMonth = viewModel.StartDateSelectedMonth,
            SelectedYear = viewModel.StartDateSelectedYear
        };

        var awardedQuestion = new DateQuestionModel()
        {
            SelectedMonth = viewModel.QualificationAwardedDate.SelectedMonth,
            SelectedYear = viewModel.QualificationAwardedDate.SelectedYear,
        };

        viewModel = HelpControllerPageMapper.MapQualificationDetailsContentToViewModel(content, ModelState, viewModel);

        var awardedValidationResult = questionModelValidator.IsValid(awardedQuestion, content.AwardedDateQuestion);

        // Validate optional field if it is not null
        if (startedQuestion.SelectedMonth is not null || startedQuestion.SelectedYear is not null)
        {
            var startedValidationResult = questionModelValidator.IsValid(startedQuestion, content.StartDateQuestion);

            if (awardedValidationResult.YearValid && questionModelValidator.DisplayAwardedDateBeforeStartDateError(startedQuestion, awardedQuestion))
            {
                awardedValidationResult.MonthValid = false;
                awardedValidationResult.YearValid = false;
                awardedValidationResult.ErrorMessages.Add(content.AwardedDateIsAfterStartedDateErrorText);
                awardedValidationResult.BannerErrorMessages.Add(new BannerError(content.AwardedDateIsAfterStartedDateErrorText, FieldId.Month));
            }

            viewModel.QuestionModel.StartedQuestion = HelpControllerPageMapper.MapDateModel(startedQuestion, content, startedValidationResult, startedQuestion.SelectedMonth, startedQuestion.SelectedYear, placeholderUpdater);

            if (viewModel.QuestionModel.StartedQuestion.MonthError || viewModel.QuestionModel.StartedQuestion.YearError)
            {
                viewModel.QuestionModel.HasErrors = true;
            }
        }

        viewModel.QuestionModel.AwardedQuestion = HelpControllerPageMapper.MapDateModel(awardedQuestion, content, awardedValidationResult, awardedQuestion.SelectedMonth, awardedQuestion.SelectedYear, placeholderUpdater);

        if (viewModel.QuestionModel.AwardedQuestion.MonthError || viewModel.QuestionModel.AwardedQuestion.YearError)
        {
            viewModel.QuestionModel.HasErrors = true;
        }

        // check form has errors or date validation failed
        if (!ModelState.IsValid || viewModel.QuestionModel.HasErrors)
        {
            if (viewModel.QuestionModel?.StartedQuestion?.ErrorSummaryLinks is not null)
            {
                viewModel.Errors.AddRange(viewModel.QuestionModel.StartedQuestion.ErrorSummaryLinks);
            }

            if (viewModel.QuestionModel?.AwardedQuestion?.ErrorSummaryLinks is not null)
            {
                viewModel.Errors.AddRange(viewModel.QuestionModel.AwardedQuestion.ErrorSummaryLinks);
            }

            return View("QualificationDetails", viewModel);
        }

        // valid submit 
        var helpCookie = userJourneyCookieService.GetHelpFormEnquiry();

        helpCookie.QualificationName = viewModel.QualificationName;
        helpCookie.QualificationStartDate = $"{viewModel.StartDateSelectedMonth}/{viewModel.StartDateSelectedYear}";
        helpCookie.QualificationAwardedDate = $"{viewModel.QualificationAwardedDate.SelectedMonth}/{viewModel.QualificationAwardedDate.SelectedYear}";
        helpCookie.AwardingOrganisation = viewModel.AwardingOrganisation;

        userJourneyCookieService.SetHelpFormEnquiry(helpCookie);

        return RedirectToAction("ProvideDetails");
    }

    [HttpGet("provide-details")]
    public async Task<IActionResult> ProvideDetailsAsync()
    {
        var content = await contentService.GetHelpProvideDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help provide details page' content could not be found");
        }

        var viewModel = HelpControllerPageMapper.MapProvideDetailsPageContentToViewModel(content, ModelState, userJourneyCookieService.GetHelpFormEnquiry().ReasonForEnquiring);

        return View("ProvideDetails", viewModel);
    }

    [HttpPost("provide-details")]
    public async Task<IActionResult> ProvideDetailsAsync([FromForm] ProvideDetailsPageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var content = await contentService.GetHelpProvideDetailsPage();

            if (content is null)
            {
                throw new Exception("'Help provide details page' content could not be found");
            }

            viewModel = HelpControllerPageMapper.MapProvideDetailsPageContentToViewModel(content, ModelState, userJourneyCookieService.GetHelpFormEnquiry().ReasonForEnquiring);

            return View("ProvideDetails", viewModel);
        }

        // valid submit
        var helpCookie = userJourneyCookieService.GetHelpFormEnquiry();

        helpCookie.AdditionalInformation = viewModel.ProvideAdditionalInformation;

        userJourneyCookieService.SetHelpFormEnquiry(helpCookie);

        return RedirectToAction("EmailAddress");
    }

    [HttpGet("email-address")]
    public async Task<IActionResult> EmailAddressAsync()
    {
        var content = await contentService.GetHelpEmailAddressPage();

        if (content is null)
        {
            throw new Exception("'Help email address page' content could not be found");
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
                throw new Exception("'Help email address page' content could not be found");
            }

            viewModel = HelpControllerPageMapper.MapEmailAddressPageContentToViewModel(content, ModelState);

            return View("EmailAddress", viewModel);
        }

        // send help form email
        /*notificationService.SendHelpPageNotification(
            new HelpPageNotification(viewModel.EmailAddress, userJourneyCookieService.GetHelpFormEnquiry())
        );
*/
        // clear data collected from help form on successful submit
        userJourneyCookieService.SetHelpFormEnquiry(new());

        return RedirectToAction("Confirmation");
    }

    [HttpGet("confirmation")]
    public async Task<IActionResult> ConfirmationAsync()
    {
        var helpConfirmationPage = await contentService.GetHelpConfirmationPage();

        if (helpConfirmationPage is null)
        {
            throw new Exception("'Help confirmation page' content could not be found");
        }

        var viewModel = await HelpControllerPageMapper.MapConfirmationPageContentToViewModelAsync(helpConfirmationPage, contentParser);

        return View("Confirmation", viewModel);
    }
}
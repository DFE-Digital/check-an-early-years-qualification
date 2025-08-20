using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/help")]
public class HelpController(
    ILogger<HelpController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IDateQuestionModelValidator questionModelValidator
    )
    : ServiceController
{
    public override void OnActionExecuting(ActionExecutingContext context) //todo check this?
    {
        // user has landed on an Help page, so is not going to select a qualification from the list,
        // partly determines where the back button should go when displaying qualification details
        userJourneyCookieService.SetUserSelectedQualificationFromList(YesOrNo.No);
        userJourneyCookieService.ClearAdditionalQuestionsAnswers();

        base.OnActionExecuting(context);
    }

    [HttpGet("get-help")]
    public IActionResult Help()
    {
        var model = new NewHelpPageViewModel();

        return View("NewHelp", model);
    }


    [HttpPost("get-help")]
    public IActionResult Help([FromForm] NewHelpPageViewModel model)
    {
        if (ModelState.IsValid)
        {
            var reasonForEnquiring = ModelState.FirstOrDefault().Value?.RawValue?.ToString();

            switch (reasonForEnquiring)
            {
                case "QuestionAboutAQualification":
                    userJourneyCookieService.SetReasonForEnquiringHelpForm(
                        new()
                        {
                            ReasonForEnquiring = "QuestionAboutAQualification",
                        }
                    );
                    return RedirectToAction("QualificationDetails");
                case "IssueWithTheService":
                    userJourneyCookieService.SetReasonForEnquiringHelpForm(
                        new()
                        {
                            ReasonForEnquiring = "IssueWithTheService",
                        }
                    );
                    return RedirectToAction("ProvideDetails");
                default:
                    throw new InvalidOperationException("Unexpected enquiry option selected"); //todo check this should we redirect home if someone tampers with html?
            }
        }

        model.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0);

        return View("NewHelp", model);
    }

    [HttpGet("qualification-details")]
    public IActionResult QualificationDetails()
    {
        var model = new QualificationDetailsViewModel();

        var qualificationStart = userJourneyCookieService.GetWhenWasQualificationStarted();
        model.OptionalQualificationStartDate.SelectedMonth = qualificationStart.startMonth;
        model.OptionalQualificationStartDate.SelectedYear = qualificationStart.startYear;

        var qualificationAwarded = userJourneyCookieService.GetWhenWasQualificationAwarded();
        model.QualificationAwardedDate.SelectedMonth = qualificationAwarded.startMonth;
        model.QualificationAwardedDate.SelectedYear = qualificationAwarded.startYear;

        return View("QualificationDetails", model);
    }

    [HttpPost("qualification-details")]
    public IActionResult QualificationDetails([FromForm] QualificationDetailsViewModel model)
    {
        if (ModelState.IsValid)
        {
            var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            helpCookie.QualificationName = model.QualificationName;
            helpCookie.QualificationStartDate = $"{model.OptionalQualificationStartDate.SelectedMonth}/{model.OptionalQualificationStartDate.SelectedYear}";  // todo check this as can be null
            helpCookie.QualificationAwardedDate = $"{model.QualificationAwardedDate.SelectedMonth}/{model.QualificationAwardedDate.SelectedYear}";
            helpCookie.AwardingOrganisation = model.AwardingOrganisation;

            userJourneyCookieService.SetReasonForEnquiringHelpForm(helpCookie);

            return RedirectToAction("ProvideDetails");
        }

        model.HasQualificationNameError = ModelState.Keys.Any(_ => ModelState["QualificationName"]?.Errors.Count > 0);
        model.HasAwardingOrganisationError = ModelState.Keys.Any(_ => ModelState["AwardingOrganisation"]?.Errors.Count > 0);

        model.OptionalQualificationStartDate.SelectedMonth = 1;
        model.OptionalQualificationStartDate.SelectedYear = 2000;
        model.QualificationAwardedDate.SelectedMonth = 1;
        model.QualificationAwardedDate.SelectedYear = 2022;

        return View("QualificationDetails", model);
    }

    [HttpGet("provide-details")]
    public IActionResult ProvideDetails()
    {
        var model = new ProvideDetailsViewModel();

        return View("ProvideDetails", model);
    }

    [HttpPost("provide-details")]
    public IActionResult ProvideDetails([FromForm] ProvideDetailsViewModel model)
    {
        if (ModelState.IsValid)
        {
            var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            helpCookie.AdditionalInformation = model.ProvideAdditionalInformation;

            userJourneyCookieService.SetReasonForEnquiringHelpForm(helpCookie);

            return RedirectToAction("EmailAddress");
        }

        model.HasAdditionalInformationError = ModelState.Keys.Any(_ => ModelState["ProvideAdditionalInformation"]?.Errors.Count > 0);

        return View("ProvideDetails", model);
    }

    [HttpGet("email-address")]
    public IActionResult EmailAddress()
    {
        var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

        var model = new EmailAddressViewModel();

        return View("EmailAddress", model);
    }

    [HttpPost("email-address")]
    public IActionResult EmailAddress([FromForm] EmailAddressViewModel model)
    {
        if (ModelState.IsValid)
        {
            var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            //todo clear cookie?

            /*notificationService.SendHelpPageNotification(new HelpPageNotification
            {
                EmailAddress = model.EmailAddress,
                Subject = helpCookie.ReasonForEnquiring,
                Message = $"QualificationName: {helpCookie.QualificationName} StartDate: {helpCookie.QualificationStartDate} " +
                $"AwardedDate: {helpCookie.QualificationAwardedDate} AwardingOrganisation: {helpCookie.AwardingOrganisation}  " +
                $"ReasonForEnquiring: {helpCookie.ReasonForEnquiring} AdditionalInformation: {helpCookie.AdditionalInformation}"
            });*/

            return RedirectToAction("Confirmation");
        }

        model.HasEmailAddressError = ModelState.Keys.Any(_ => ModelState["EmailAddress"]?.Errors.Count > 0);
        
        return View("EmailAddress", model);
    }

    [HttpGet("confirmation")]
    public IActionResult Confirmation()
    {
        var model = new ConfirmationViewModel();

        return View("Confirmation", model);
    }

    [HttpPost("confirmation")]
    public IActionResult Confirmation([FromForm] ConfirmationViewModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Confirmation");
        }

        return View("Confirmation", model);
    }
}
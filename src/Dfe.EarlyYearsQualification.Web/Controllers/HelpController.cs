using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
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
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IDateQuestionModelValidator questionModelValidator,
    IHelpGetHelpPageMapper getHelpPageMapper,
    IHelpQualificationDetailsPageMapper helpQualificationDetailsPageMapper,
    IHelpProvideDetailsPageMapper helpProvideDetailsPageMapper,
    IHelpEmailAddressPageMapper helpEmailAddressPageMapper,
    IHelpConfirmationPageMapper helpConfirmationPageMapper
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

        var viewModel = await getHelpPageMapper.MapGetHelpPageContentToViewModelAsync(content);

        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is not null)
        {
            viewModel.SelectedOption = 
                enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.QuestionAboutAQualification ? nameof(HelpFormEnquiryReasons.QuestionAboutAQualification) : 
                enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.IssueWithTheService ? nameof(HelpFormEnquiryReasons.IssueWithTheService) : "";
        }

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
            var viewModel = await getHelpPageMapper.MapGetHelpPageContentToViewModelAsync(content);
            viewModel.HasNoEnquiryOptionSelectedError = ModelState.Keys.Any(_ => ModelState["SelectedOption"]?.Errors.Count > 0) || !submittedValueIsValid;

            return View("GetHelp", viewModel);
        }
        
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry() ?? new();

        // valid submit
        switch (model.SelectedOption)
        {

            case nameof(HelpFormEnquiryReasons.QuestionAboutAQualification):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.QuestionAboutAQualification;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(QualificationDetails));
            case nameof(HelpFormEnquiryReasons.IssueWithTheService):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.IssueWithTheService;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
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

        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        // set any previously entered qualification details from cookie
        viewModel.AwardingOrganisation = enquiry.AwardingOrganisation ?? userJourneyCookieService.GetAwardingOrganisation() ?? "";
        viewModel.QualificationName = enquiry.QualificationName ?? userJourneyCookieService.GetSelectedQualificationName() ?? "";

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

        if (enquiry.QualificationStartDate is not null)
        {
            var enquiryStart = StringDateHelper.SplitDate(enquiry.QualificationStartDate);

            viewModel.QuestionModel.StartedQuestion.SelectedMonth = enquiryStart.startMonth;
            viewModel.QuestionModel.StartedQuestion.SelectedYear = enquiryStart.startYear;
        }

        if (enquiry.QualificationAwardedDate is not null)
        {
            var enquiryAwarded = StringDateHelper.SplitDate(enquiry.QualificationAwardedDate);

            viewModel.QuestionModel.AwardedQuestion.SelectedMonth = enquiryAwarded.startMonth;
            viewModel.QuestionModel.AwardedQuestion.SelectedYear = enquiryAwarded.startYear;
        }

        viewModel = helpQualificationDetailsPageMapper.MapQualificationDetailsContentToViewModel(viewModel, content, null, ModelState);

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

        var hasInvalidDates = !datesValidationResult.AwardedValidationResult!.MonthValid || !datesValidationResult.AwardedValidationResult.YearValid ||
            (datesValidationResult.StartedValidationResult is not null && (!datesValidationResult.StartedValidationResult.MonthValid || !datesValidationResult.StartedValidationResult.YearValid));

        if (!ModelState.IsValid || hasInvalidDates)
        {
            model.HasQualificationNameError = ModelState.Keys.Any(_ => ModelState["QualificationName"]?.Errors.Count > 0);
            model.HasAwardingOrganisationError = ModelState.Keys.Any(_ => ModelState["AwardingOrganisation"]?.Errors.Count > 0);

            model.QuestionModel.HasErrors = hasInvalidDates;

            model = helpQualificationDetailsPageMapper.MapQualificationDetailsContentToViewModel(model, content, datesValidationResult, ModelState);

            return View("QualificationDetails", model);
        }

        // valid submit 
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        enquiry.QualificationName = model.QualificationName;
        if (model.QuestionModel.StartedQuestion is not null)
        {
            enquiry.QualificationStartDate = $"{model.QuestionModel.StartedQuestion?.SelectedMonth}/{model.QuestionModel.StartedQuestion?.SelectedYear}";
        }
        enquiry.QualificationAwardedDate = $"{model.QuestionModel.AwardedQuestion?.SelectedMonth}/{model.QuestionModel.AwardedQuestion?.SelectedYear}";
        enquiry.AwardingOrganisation = model.AwardingOrganisation;

        userJourneyCookieService.SetHelpFormEnquiry(enquiry);

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

        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        var viewModel = helpProvideDetailsPageMapper.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring);

        viewModel.ProvideAdditionalInformation = enquiry.AdditionalInformation;

        return View("ProvideDetails", viewModel);
    }

    [HttpPost("provide-details")]
    public async Task<IActionResult> ProvideDetails([FromForm] ProvideDetailsPageViewModel viewModel)
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        if (!ModelState.IsValid)
        {
            var content = await contentService.GetHelpProvideDetailsPage();

            if (content is null)
            {
                logger.LogError("'Help provide details page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            viewModel = helpProvideDetailsPageMapper.MapProvideDetailsPageContentToViewModel(content, enquiry.ReasonForEnquiring);

            viewModel.HasAdditionalInformationError = ModelState.Keys.Any(_ => ModelState["ProvideAdditionalInformation"]?.Errors.Count > 0);

            return View("ProvideDetails", viewModel);
        }

        // valid submit

        enquiry.AdditionalInformation = viewModel.ProvideAdditionalInformation;

        userJourneyCookieService.SetHelpFormEnquiry(enquiry);

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

        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        var viewModel = helpEmailAddressPageMapper.MapEmailAddressPageContentToViewModel(content);

        return View("EmailAddress", viewModel);
    }

    [HttpPost("email-address")]
    public async Task<IActionResult> EmailAddress([FromForm] EmailAddressPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var content = await contentService.GetHelpEmailAddressPage();

            if (content is null)
            {
                logger.LogError("'Help email address page' content could not be found");
                return RedirectToAction("Index", "Error");
            }

            var viewModel = helpEmailAddressPageMapper.MapEmailAddressPageContentToViewModel(content);

            viewModel.HasEmailAddressError = string.IsNullOrEmpty(model.EmailAddress) || ModelState.Keys.Any(_ => ModelState["EmailAddress"]?.Errors.Count > 0);
            viewModel.EmailAddressErrorMessage = string.IsNullOrEmpty(model.EmailAddress)
                                            ? content.NoEmailAddressEnteredErrorMessage
                                            : content.InvalidEmailAddressErrorMessage;

            return View("EmailAddress", viewModel);
        }

        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is null)
        {
            logger.LogError("Help form enquiry is null");
            return RedirectToAction("GetHelp", "Help");
        }

        // send help form email
        notificationService.SendHelpPageNotification(
            new HelpPageNotification(model.EmailAddress, enquiry)
        );

        // clear data collected from help form on successful submit
        userJourneyCookieService.SetHelpFormEnquiry(null);

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

        var viewModel = await helpConfirmationPageMapper.MapConfirmationPageContentToViewModelAsync(content);

        return View("Confirmation", viewModel);
    }
}
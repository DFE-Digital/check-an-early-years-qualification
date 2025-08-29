using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

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
    public async Task<IActionResult> GetHelpAsync() => View("NewHelp", await GetGetHelpPageContentAsync());

    [HttpPost("get-help")]
    public async Task<IActionResult> Help([FromForm] NewHelpPageViewModel model)
    {
        if (ModelState.IsValid)
        {
            switch (model.SelectedOption)
            {
                case "QuestionAboutAQualification":
                    userJourneyCookieService.SetReasonForEnquiringHelpForm(
                        new()
                        {
                            ReasonForEnquiring = "Question about a qualification",
                        }
                    );
                    return RedirectToAction("QualificationDetails");
                case "IssueWithTheService":
                    userJourneyCookieService.SetReasonForEnquiringHelpForm(
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

        // invalid submit

        var viewModel = await GetGetHelpPageContentAsync();

        return View("NewHelp", viewModel);
    }

    private async Task<NewHelpPageViewModel> GetGetHelpPageContentAsync()
    {
        var content = await contentService.GetGetHelpPage();

        if (content is null)
        {
            throw new Exception("'Get help page' content could not be found");
        }

        return await MapGetHelpPageContentToViewModelAsync(content);
    }

    // mapper todo move to mapper class
    private async Task<NewHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage helpPageContent)
    {
        var enquiryReasons = new List<EnquiryOptionModel>();
        foreach (var enquiryReason in helpPageContent.EnquiryReasons)
        {
            enquiryReasons.Add(new EnquiryOptionModel { Label = enquiryReason.Label, Value = enquiryReason.Value } );
        }

        var viewModel = new NewHelpPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = helpPageContent.BackButton.DisplayText,
                Href = helpPageContent.BackButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = await contentParser.ToHtml(helpPageContent.PostHeadingContent),
            CtaButtonText = helpPageContent.CtaButtonText,
            EnquiryReasons = enquiryReasons,
            NoEnquiryOptionSelectedErrorMessage = helpPageContent.NoEnquiryOptionSelectedErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
            ReasonForEnquiryHeading = helpPageContent.ReasonForEnquiryHeading,
        };

        viewModel.HasNoEnquiryOptionSelectedError = HasAnError(elementName: "SelectedOption");

        return viewModel;
    }











    [HttpGet("qualification-details")]
    public async Task<IActionResult> QualificationDetailsAsync() => View("QualificationDetails", await GetHelpQualificationDetailsPageContentAsync());

    [HttpPost("qualification-details")]
    public async Task<IActionResult> QualificationDetailsAsync([FromForm] QualificationDetailsPageViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            helpCookie.QualificationName = viewModel.QualificationName;
            helpCookie.QualificationStartDate = $"{viewModel.StartDateSelectedMonth}/{viewModel.StartDateSelectedYear}";  // todo check this as can be null
            helpCookie.QualificationAwardedDate = $"{viewModel.AwardedDateSelectedMonth}/{viewModel.AwardedDateSelectedYear}";
            helpCookie.AwardingOrganisation = viewModel.AwardingOrganisation;

            userJourneyCookieService.SetReasonForEnquiringHelpForm(helpCookie);

            return RedirectToAction("ProvideDetails");
        }

        // invalid submit

        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help qualification details page' content could not be found");
        }


        DatesQuestionModel e =
            new()
            {
                StartedQuestion = new DateQuestionModel()
                {
                    SelectedMonth = viewModel.StartDateSelectedMonth,
                    SelectedYear = viewModel.StartDateSelectedYear,
                },
                AwardedQuestion = new DateQuestionModel()
                {
                    SelectedMonth = viewModel.AwardedDateSelectedMonth,
                    SelectedYear = viewModel.AwardedDateSelectedYear,
                },
            };


        var dateModelValidationResult = questionModelValidator.StartDateIsOptionalIsValid(content, viewModel);

   /*     // todo map error messages to vm
        if (!dateModelValidationResult.Valid)
        {
            var model = MapDatesModel(e, content, "action name", "controller name", dateModelValidationResult);
        }*/


        viewModel = await GetHelpQualificationDetailsPageContentAsync();


        return View("QualificationDetails", viewModel);
    }

/*

    private DatesQuestionModel MapDatesModel(DatesQuestionModel model, HelpQualificationDetailsPage content, string actionName, string controllerName, DatesValidationResult? validationResult)
    {
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var startedModel = MapDateModel(new DateQuestionModel(), content.StartedQuestion!,
                                        validationResult?.StartedValidationResult, startMonth, startYear);

        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardedModel = MapDateModel(new DateQuestionModel(), content.AwardedQuestion!,
                                        validationResult?.AwardedValidationResult, awardedMonth, awardedYear);

        return DatesQuestionMapper.Map(model, content, actionName, controllerName, startedModel, awardedModel);
    }

    private DateQuestionModel MapDateModel(DateQuestionModel model, HelpQualificationDetailsPage content,
                                            DateValidationResult? validationResult,
                                            int? selectedMonth,
                                            int? selectedYear)
    {
        var bannerErrors = validationResult is { BannerErrorMessages.Count: > 0 } ? validationResult.BannerErrorMessages : null;

        var errorMessageText = validationResult is { ErrorMessages.Count: > 0 }
                                   ? string.Join("<br />", validationResult.ErrorMessages)
                                   : null;

        var errorBannerMessages = new List<BannerError>();
        if (bannerErrors is null)
        {
            errorBannerMessages.Add(new BannerError(content.ErrorMessage, FieldId.Month));
        }
        else
        {
            foreach (var bannerError in bannerErrors)
            {
                errorBannerMessages.Add(new BannerError(placeholderUpdater.Replace(bannerError.Message), bannerError.FieldId));
            }
        }

        var errorMessage = placeholderUpdater.Replace(errorMessageText ?? question.ErrorMessage);

        return DateQuestionMapper.Map(model, question, errorBannerMessages, errorMessage, validationResult,
                                      selectedMonth, selectedYear);
    }




*/
    private async Task<QualificationDetailsPageViewModel> GetHelpQualificationDetailsPageContentAsync()
    {
        var content = await contentService.GetHelpQualificationDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help qualification details page' content could not be found");
        }

        return MapQualificationDetailsContentToViewModel(content);
    }

    // mapper todo move to mapper class
    private QualificationDetailsPageViewModel MapQualificationDetailsContentToViewModel(HelpQualificationDetailsPage helpPageContent)
    {
        var viewModel = new QualificationDetailsPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = helpPageContent.BackButton.DisplayText,
                Href = helpPageContent.BackButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = helpPageContent.PostHeadingContent,
            CtaButtonText = helpPageContent.CtaButtonText,
            QualificationNameHeading = helpPageContent.QualificationNameHeading,
            QualificationNameErrorMessage = helpPageContent.QualificationNameErrorMessage,
            AwardingOrganisationHeading = helpPageContent.AwardingOrganisationHeading,
            AwardingOrganisationErrorMessage = helpPageContent.AwardingOrganisationErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
         
        };

        // get any previously entered values from cookie
        var qualificationStart = userJourneyCookieService.GetWhenWasQualificationStarted();
        viewModel.StartDateSelectedMonth = qualificationStart.startMonth;
        viewModel.StartDateSelectedYear = qualificationStart.startYear;

        var qualificationAwarded = userJourneyCookieService.GetWhenWasQualificationAwarded();
        viewModel.AwardedDateSelectedMonth = qualificationAwarded.startMonth;
        viewModel.AwardedDateSelectedYear = qualificationAwarded.startYear;

        viewModel.AwardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();

        viewModel.QualificationName = userJourneyCookieService.GetSelectedQualificationName();

        //Diploma of Higher Education Early Childhood Studies

        if (!ModelState.IsValid)
        {
            viewModel.HasQualificationNameError = HasAnError(elementName: "QualificationName");
            viewModel.HasAwardingOrganisationError = HasAnError(elementName: "AwardingOrganisation");
            viewModel.HasRequiredAwardedDateMonthError = HasAnError(elementName: "AwardedDateSelectedMonth");
            viewModel.HasRequiredAwardedDateYearError = HasAnError(elementName: "AwardedDateSelectedYear");
        }

        return viewModel;
    }










    [HttpGet("provide-details")]
    public async Task<IActionResult> ProvideDetailsAsync() => View("ProvideDetails", await GetHelpProvideDetailsPageContentAsync());

    [HttpPost("provide-details")]
    public async Task<IActionResult> ProvideDetailsAsync([FromForm] ProvideDetailsPageViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var helpCookie = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            helpCookie.AdditionalInformation = viewModel.ProvideAdditionalInformation;

            userJourneyCookieService.SetReasonForEnquiringHelpForm(helpCookie);

            return RedirectToAction("EmailAddress");
        }

        // invalid submit

        viewModel = await GetHelpProvideDetailsPageContentAsync();

        return View("ProvideDetails", viewModel);
    }

    private async Task<ProvideDetailsPageViewModel> GetHelpProvideDetailsPageContentAsync()
    {
        var content = await contentService.GetHelpProvideDetailsPage();

        if (content is null)
        {
            throw new Exception("'Help provide details page' content could not be found");
        }

        return MapProvideDetailsPageContentToViewModel(content);
    }

    // mapper todo move to mapper class
    private ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage helpPageContent)
    {
        var backButton = userJourneyCookieService.GetReasonForEnquiringHelpForm().ReasonForEnquiring == "Question about a qualification"
                             ? helpPageContent.BackButtonToQualificationDetailsPage
                             : helpPageContent.BackButtonToGetHelpPage;

        var viewModel = new ProvideDetailsPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = backButton.DisplayText,
                Href = backButton.Href
            },
            Heading = helpPageContent.Heading,
            PostHeadingContent = helpPageContent.PostHeadingContent,
            CtaButtonText = helpPageContent.CtaButtonText,
            AdditionalInformationWarningText = helpPageContent.AdditionalInformationWarningText,
            AdditionalInformationErrorMessage = helpPageContent.AdditionalInformationErrorMessage,
            ErrorBannerHeading = helpPageContent.ErrorBannerHeading,
        };

        if (!ModelState.IsValid)
        {
            viewModel.HasAdditionalInformationError = HasAnError(elementName: "ProvideAdditionalInformation");
        }

        return viewModel;
    }
















    [HttpGet("email-address")]
    public async Task<IActionResult> EmailAddressAsync() => View("EmailAddress", await GetHelpEmailAddressPageContentAsync());

    [HttpPost("email-address")]
    public async Task<IActionResult> EmailAddress([FromForm] EmailAddressPageViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var enquiry = userJourneyCookieService.GetReasonForEnquiringHelpForm();

            //todo email address may not be needed on cookie
            enquiry.EmailAddress = viewModel.EmailAddress;

            // uncomment this when ready to send email
            /*notificationService.SendHelpPageNotification(new HelpPageNotification
            {
                EmailAddress = model.EmailAddress,
                Subject = enquiry.ReasonForEnquiring,
                Message = ConstructMessage(enquiry)
            });*/

            // clear data collected from help form on successful submit
            userJourneyCookieService.SetReasonForEnquiringHelpForm(new());

            return RedirectToAction("Confirmation");
        }

        // invalid submit

        viewModel = await GetHelpEmailAddressPageContentAsync();

        return View("EmailAddress", viewModel);
    }

    private async Task<EmailAddressPageViewModel> GetHelpEmailAddressPageContentAsync()
    {
        var content = await contentService.GetHelpEmailAddressPage();

        if (content is null)
        {
            throw new Exception("'Help email address page' content could not be found");
        }

        return MapEmailAddressPageContentToViewModel(content);
    }

    // mapper todo move to mapper class
    private EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage emailAddressPageContent)
    {
        var viewModel = new EmailAddressPageViewModel()
        {
            BackButton = new()
            {
                DisplayText = emailAddressPageContent.BackButton.DisplayText,
                Href = emailAddressPageContent.BackButton.Href
            },
            Heading = emailAddressPageContent.Heading,
            PostHeadingContent = emailAddressPageContent.PostHeadingContent,
            CtaButtonText = emailAddressPageContent.CtaButtonText,
            ErrorBannerHeading = emailAddressPageContent.ErrorBannerHeading,
        };

        if (!ModelState.IsValid)
        {
            viewModel.HasEmailAddressError = string.IsNullOrEmpty(viewModel.EmailAddress) || ModelState.Keys.Any(_ => ModelState["EmailAddress"]?.Errors.Count > 0);

            viewModel.EmailAddressErrorMessage = string.IsNullOrEmpty(viewModel.EmailAddress)
                                            ? emailAddressPageContent.NoEmailAddressEnteredErrorMessage
                                            : emailAddressPageContent.InvalidEmailAddressErrorMessage;
        }

        return viewModel;
    }


    // todo move to service
    private string ConstructMessage(UserJourneyCookieService.HelpFormEnquiry enquiry)
    {
        var message = new StringBuilder();

        message.AppendLine();
        message.AppendLine($"Reason for enquiring: {enquiry.ReasonForEnquiring}");

        message.AppendLine();

        if (!string.IsNullOrEmpty(enquiry.QualificationName))
        {
            message.AppendLine($"Qualification name: {enquiry.QualificationName}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationStartDate))
        {
            message.AppendLine($"Qualification start date: {enquiry.QualificationStartDate}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationAwardedDate))
        {
            message.AppendLine($"Qualification awarded date: {enquiry.QualificationAwardedDate}");
            message.AppendLine();
        }

        if (!string.IsNullOrEmpty(enquiry.AwardingOrganisation))
        {
            message.AppendLine($"Awarding organisation: {enquiry.AwardingOrganisation}");
            message.AppendLine();
        }

        message.AppendLine($"Additional information: {enquiry.AdditionalInformation}");
        message.AppendLine();

       /* message.AppendLine($"Email address: {enquiry.EmailAddress}");
        message.AppendLine();*/

        return message.ToString();
    }









    [HttpGet("confirmation")]
    public async Task<IActionResult> ConfirmationAsync() => View("Confirmation", await GetConfirmationPageContentAsync());

    private async Task<ConfirmationPageViewModel> GetConfirmationPageContentAsync()
    {
        var helpConfirmationPage = await contentService.GetHelpConfirmationPage();
        if (helpConfirmationPage is null)
        {
            throw new Exception("'Help confirmation page' content could not be found");
        }

        return await Map(helpConfirmationPage);
    }

    private async Task<ConfirmationPageViewModel> Map(HelpConfirmationPage helpConfirmationPage)
    {
        var bodyHtml = await contentParser.ToHtml(helpConfirmationPage.Body);
        var feedbackBodyHtml = await contentParser.ToHtml(helpConfirmationPage.FeedbackComponent!.Body);
        return HelpConfirmationPageModelMapper.Map(helpConfirmationPage, bodyHtml, feedbackBodyHtml);
    }

    private bool HasAnError(string elementName) => ModelState.Keys.Any(_ => ModelState[elementName]?.Errors.Count > 0);
}
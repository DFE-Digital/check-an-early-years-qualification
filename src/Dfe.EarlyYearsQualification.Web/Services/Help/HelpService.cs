using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public class HelpService(
    ILogger<HelpService> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IDateQuestionModelValidator questionModelValidator,
    IHelpGetHelpPageMapper getHelpPageMapper,
    IHelpProceedWithQualificationQueryPageMapper proceedWithQualificationQueryPageMapper,
    IHelpQualificationDetailsPageMapper helpQualificationDetailsPageMapper,
    IHelpProvideDetailsPageMapper helpProvideDetailsPageMapper,
    IHelpEmailAddressPageMapper helpEmailAddressPageMapper,
    IHelpConfirmationPageMapper helpConfirmationPageMapper,
    IStaticPageMapper staticPageMapper
) : ServiceController, IHelpService
{
    public async Task<GetHelpPage?> GetGetHelpPageAsync()
    {
        return await contentService.GetGetHelpPage();
    }

    public async Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage content)
    {
        return await getHelpPageMapper.MapGetHelpPageContentToViewModelAsync(content);
    }

    public string GetWhyAreYouContactingUsSelectedOption()
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is not null)
        {
            switch (enquiry.ReasonForEnquiring)
            {
                case HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript:
                    return nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript);
                case HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs:
                    return nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs);
                case HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol:
                    return nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol);
                case HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification:
                    return nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification);
                case HelpFormEnquiryReasons.GetHelp.IssueWithTheService:
                    return nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService);
                default:
                    return string.Empty;
            }
        }

        return "";
    }

    public string GetWhatDoYouWantToDoNextSelectedOption()
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        if (enquiry is not null)
        {
            switch (enquiry.ReasonForEnquiring)
            {
                case HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService:
                return nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.CheckTheQualificationUsingTheService);
                case HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam:
                return nameof(HelpFormEnquiryReasons.ProceedWithQualificationQuery.ContactTheEarlyYearsQualificationTeam);
                default:
                return string.Empty;
            }
        }

        return "";
    }

    public bool SelectedOptionIsValid(List<EnquiryOption> options, string value)
    {
        return options.Select(x => x.Value).Contains(value);
    }

    public RedirectToActionResult SetHelpFormEnquiryReason(GetHelpPageViewModel model)
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry() ?? new();

        switch (model.SelectedOption)
        {
            case nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(HelpController.INeedACopyOfTheQualificationCertificateOrTranscript));
            case nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(HelpController.IDoNotKnowWhatLevelTheQualificationIs));
            case nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(HelpController.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol));
            case nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(HelpController.ProceedWithQualificationQuery));
            case nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService):
                enquiry.ReasonForEnquiring = HelpFormEnquiryReasons.GetHelp.IssueWithTheService;
                userJourneyCookieService.SetHelpFormEnquiry(enquiry);
                return RedirectToAction(nameof(HelpController.ProvideDetails));
            default:
                logger.LogError("Unexpected enquiry option");
                return RedirectToAction("Index", "Error");
        }
    }

    public async Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPageAsync()
    {
        return await contentService.GetHelpQualificationDetailsPage();
    }

    public async Task<HelpProceedWithQualificationQueryPage?> GetProceedWithQualificationQueryPageAsync()
    {
        return await contentService.GetProceedWithQualificationQueryPage();
    }

    public async Task<ProceedWithQualificationQueryViewModel> MapProceedWithQualificationQueryPageContentToViewModelAsync(HelpProceedWithQualificationQueryPage content)
    {
        return await proceedWithQualificationQueryPageMapper.MapProceedWithQualificationQueryPageContentToViewModelAsync(content);
    }

    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie(QualificationDetailsPageViewModel viewModel)
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        viewModel.AwardingOrganisation = enquiry.AwardingOrganisation;
        viewModel.QualificationName = enquiry.QualificationName;

        var qualificationStart = userJourneyCookieService.GetWhenWasQualificationStarted();
        var qualificationAwarded = userJourneyCookieService.GetWhenWasQualificationAwarded();

        viewModel.QuestionModel.StartedQuestion = new DateQuestionModel
                                                  {
                                                      SelectedMonth = qualificationStart.startMonth,
                                                      SelectedYear = qualificationStart.startYear
                                                  };

        viewModel.QuestionModel.AwardedQuestion = new DateQuestionModel
                                                  {
                                                      SelectedMonth = qualificationAwarded.startMonth,
                                                      SelectedYear = qualificationAwarded.startYear
                                                  };

        if (!string.IsNullOrEmpty(enquiry.QualificationStartDate))
        {
            var enquiryStart = StringDateHelper.SplitDate(enquiry.QualificationStartDate);

            viewModel.QuestionModel.StartedQuestion.SelectedMonth = enquiryStart.startMonth;
            viewModel.QuestionModel.StartedQuestion.SelectedYear = enquiryStart.startYear;
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationAwardedDate))
        {
            var enquiryAwarded = StringDateHelper.SplitDate(enquiry.QualificationAwardedDate);

            viewModel.QuestionModel.AwardedQuestion.SelectedMonth = enquiryAwarded.startMonth;
            viewModel.QuestionModel.AwardedQuestion.SelectedYear = enquiryAwarded.startYear;
        }
    }

    public QualificationDetailsPageViewModel MapHelpQualificationDetailsPageContentToViewModel(
        QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content,
        DatesValidationResult? validationResult, ModelStateDictionary modelState)
    {
        return helpQualificationDetailsPageMapper.MapQualificationDetailsContentToViewModel(viewModel, content,
         validationResult, modelState);
    }

    public void SetHelpQualificationDetailsInCookie(HelpFormEnquiry enquiry,
                                                    QualificationDetailsPageViewModel viewModel)
    {
        enquiry.QualificationName = viewModel.QualificationName;
        if (viewModel.QuestionModel.StartedQuestion is not null)
        {
            enquiry.QualificationStartDate =
                $"{viewModel.QuestionModel.StartedQuestion?.SelectedMonth}/{viewModel.QuestionModel.StartedQuestion?.SelectedYear}";
        }

        enquiry.QualificationAwardedDate =
            $"{viewModel.QuestionModel.AwardedQuestion?.SelectedMonth}/{viewModel.QuestionModel.AwardedQuestion?.SelectedYear}";
        enquiry.AwardingOrganisation = viewModel.AwardingOrganisation;

        SetHelpFormEnquiry(enquiry);
    }

    public bool HasInvalidDates(DatesValidationResult datesValidationResult)
    {
        return !datesValidationResult.AwardedValidationResult!.MonthValid ||
               !datesValidationResult.AwardedValidationResult.YearValid ||
               (datesValidationResult.StartedValidationResult is not null &&
                (!datesValidationResult.StartedValidationResult.MonthValid ||
                 !datesValidationResult.StartedValidationResult.YearValid));
    }

    public DatesValidationResult ValidateDates(DatesQuestionModel questionModel, HelpQualificationDetailsPage content)
    {
        return questionModelValidator.IsValid(questionModel, content);
    }

    public async Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage()
    {
        return await contentService.GetHelpProvideDetailsPage();
    }

    public ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(
        HelpProvideDetailsPage content, string reasonForEnquiring)
    {
        return helpProvideDetailsPageMapper.MapProvideDetailsPageContentToViewModel(content, reasonForEnquiring);
    }

    public async Task<HelpEmailAddressPage?> GetHelpEmailAddressPage()
    {
        return await contentService.GetHelpEmailAddressPage();
    }

    public EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content)
    {
        return helpEmailAddressPageMapper.MapEmailAddressPageContentToViewModel(content);
    }

    public void SendHelpPageNotification(HelpPageNotification helpPageNotification)
    {
        notificationService.SendHelpPageNotification(helpPageNotification);
    }

    public async Task<HelpConfirmationPage?> GetHelpConfirmationPage()
    {
        return await contentService.GetHelpConfirmationPage();
    }

    public Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage content)
    {
        return helpConfirmationPageMapper.MapConfirmationPageContentToViewModelAsync(content);
    }

    public HelpFormEnquiry GetHelpFormEnquiry()
    {
        return userJourneyCookieService.GetHelpFormEnquiry();
    }

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry)
    {
        userJourneyCookieService.SetHelpFormEnquiry(formEnquiry);
    }

    public async Task<StaticPage?> GetStaticPage(string entryId)
    {
        return await contentService.GetStaticPage(entryId);
    }

    public async Task<StaticPageModel> MapStaticPage(StaticPage page)
    {
        return await staticPageMapper.Map(page);
    }
}
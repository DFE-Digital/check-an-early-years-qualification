using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces.Help;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public class HelpService(
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    INotificationService notificationService,
    IDateQuestionModelValidator questionModelValidator,
    IRadioQuestionHelpPageMapper RadioQuestionHelpPageMapper,
    IHelpQualificationDetailsPageMapper helpQualificationDetailsPageMapper,
    IHelpProvideDetailsPageMapper helpProvideDetailsPageMapper,
    IHelpEmailAddressPageMapper helpEmailAddressPageMapper,
    IHelpConfirmationPageMapper helpConfirmationPageMapper,
    IStaticPageMapper staticPageMapper,
    IPlaceholderUpdater placeholderUpdater
) : ServiceController, IHelpService
{

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
            switch (enquiry.WhatDoYouWantToDoNext)
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

    public bool SelectedOptionIsValid(List<Option> options, string value)
    {
        return options.Select(x => x.Value).Contains(value);
    }

    public void SetHelpFormEnquiryReason(string selectedOption)
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry() ?? new();

        enquiry.ReasonForEnquiring = selectedOption switch
        {
            nameof(HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript) => HelpFormEnquiryReasons.GetHelp.INeedACopyOfTheQualificationCertificateOrTranscript,
            nameof(HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs) => HelpFormEnquiryReasons.GetHelp.IDoNotKnowWhatLevelTheQualificationIs,
            nameof(HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol) => HelpFormEnquiryReasons.GetHelp.IWantToCheckWhetherACourseIsApprovedBeforeIEnrol,
            nameof(HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification) => HelpFormEnquiryReasons.GetHelp.QuestionAboutAQualification,
            nameof(HelpFormEnquiryReasons.GetHelp.IssueWithTheService) => HelpFormEnquiryReasons.GetHelp.IssueWithTheService,
            _ => string.Empty,
        };

        userJourneyCookieService.SetHelpFormEnquiry(enquiry);
    }

    public async Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPageAsync()
    {
        return await contentService.GetHelpQualificationDetailsPage();
    }

    public async Task<RadioQuestionHelpPage?> GetRadioQuestionHelpPageAsync(string entryId)
    {
        return await contentService.GetRadioQuestionHelpPage(entryId);
    }

    public async Task<RadioQuestionHelpPageViewModel> MapRadioQuestionHelpPageContentToViewModelAsync(RadioQuestionHelpPage content)
    {
        return await RadioQuestionHelpPageMapper.MapRadioQuestionHelpPageContentToViewModelAsync(content);
    }

    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content)
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        viewModel.AwardingOrganisation = enquiry.AwardingOrganisation;
        viewModel.QualificationName = enquiry.QualificationName;

        if (!string.IsNullOrEmpty(enquiry.QualificationStartDate))
        {
            var (startMonth, startYear) = StringDateHelper.SplitDate(enquiry.QualificationStartDate);

            if (startMonth is not null && startYear is not null)
            {
                var date = new DateOnly(startYear.Value, startMonth.Value, 1);
                if (date < new DateOnly(2014, 9, 1))
                {
                    viewModel.Option = content.BeforeSeptember2014Option.Value;
                }
                else
                {
                    viewModel.Option = viewModel.RadioButtonWithDateInputModel.Value;

                    if (viewModel.RadioButtonWithDateInputModel.Question is not null)
                    {
                        viewModel.RadioButtonWithDateInputModel.Question.SelectedMonth = startMonth.Value;
                        viewModel.RadioButtonWithDateInputModel.Question.SelectedYear = startYear.Value;
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(enquiry.QualificationAwardedDate))
        {
            var enquiryAwarded = StringDateHelper.SplitDate(enquiry.QualificationAwardedDate);

            viewModel.AwardedDate.SelectedMonth = enquiryAwarded.startMonth;
            viewModel.AwardedDate.SelectedYear = enquiryAwarded.startYear;
        }
    }

    public QualificationDetailsPageViewModel MapHelpQualificationDetailsPageContentToViewModel(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content)
    {
        return helpQualificationDetailsPageMapper.MapQualificationDetailsContentToViewModel(viewModel, content);
    }

    public void SetHelpQualificationDetailsInCookie(QualificationDetailsPageViewModel model)
    {
        var enquiry = GetHelpFormEnquiry();

        enquiry.QualificationName = model.QualificationName;

        enquiry.QualificationStartDate = model.Option == model.Before2014Option.Value
                    ? "1/1900"
                    : $"{model.RadioButtonWithDateInputModel.Question.SelectedMonth}/{model.RadioButtonWithDateInputModel.Question.SelectedYear}";

        enquiry.QualificationAwardedDate = $"{model.AwardedDate.SelectedMonth}/{model.AwardedDate.SelectedYear}";

        enquiry.AwardingOrganisation = model.AwardingOrganisation;

        SetHelpFormEnquiry(enquiry);
    }

    public DatesValidationResult ValidateDates(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage content)
    {
        return questionModelValidator.IsValid(model, content);
    }

    public async Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage()
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        var entryId = enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.GetHelp.IssueWithTheService
            ? HelpPages.TechnicalIssueProvideDetails
            : HelpPages.HowCanWeHelpYouProvideDetails;

        return await contentService.GetHelpProvideDetailsPage(entryId);
    }

    public ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(
        HelpProvideDetailsPage content, string reasonForEnquiring)
    {
        return helpProvideDetailsPageMapper.MapProvideDetailsPageContentToViewModel(content, reasonForEnquiring);
    }

    public async Task<HelpEmailAddressPage?> GetHelpEmailAddressPage()
    {
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        var entryId = enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.GetHelp.IssueWithTheService
            ? HelpPages.TechnicalIssueEmailAddress
            : HelpPages.QualificationQueryEmailAddress;

        return await contentService.GetHelpEmailAddressPage(entryId);
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
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();

        var entryId = enquiry.ReasonForEnquiring == HelpFormEnquiryReasons.GetHelp.IssueWithTheService
            ? HelpPages.TechnicalIssueConfirmation
            : HelpPages.QualificationQueryConfirmation;

        return await contentService.GetHelpConfirmationPage(entryId);
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

    public async Task<StaticPageModel?> MapStaticPage(StaticPage page)
    {
        return await staticPageMapper.Map(page);
    }

    public DateValidationResult DateIsValid(DateQuestionModel model, DateQuestion content)
    {
        return questionModelValidator.IsValid(model, content);
    }

    public DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question, DateValidationResult validationResult, string objectName)
    {
        var validationInfo = MapValidationResultToBanners(question, validationResult);

        DateQuestionMapper.Map(model, question, validationInfo.banners, validationInfo.errorMessage, validationResult, model.SelectedMonth, model.SelectedYear);

        ReplaceBannerDefaultIdWithElementId(model, objectName);

        return model;
    }

    private static void ReplaceBannerDefaultIdWithElementId(DateQuestionModel model, string objectName)
    {
        foreach (var errorSummaryLink in model.ErrorSummaryLinks)
        {
            errorSummaryLink.ElementLinkId = errorSummaryLink.ElementLinkId switch
            {
                nameof(FieldId.Month) => $"{objectName}.SelectedMonth",
                nameof(FieldId.Year) => $"{objectName}.SelectedYear",
                _ => errorSummaryLink.ElementLinkId
            };
        }
    }

    private (List<BannerError> banners, string errorMessage) MapValidationResultToBanners(DateQuestion question, DateValidationResult validationResult)
    {
        var errorMessageText = validationResult.ErrorMessages.Count != 0
                                   ? string.Join("<br />", validationResult.ErrorMessages)
                                   : null;

        var errorBannerMessages = new List<BannerError>();

        foreach (var bannerError in validationResult.BannerErrorMessages)
        {
            errorBannerMessages.Add(new BannerError(placeholderUpdater.Replace(bannerError.Message), bannerError.FieldId));
        }

        return (errorBannerMessages, placeholderUpdater.Replace(errorMessageText ?? question.ErrorMessage));
    }

    public void AddQualificationDetailsValidationErrors(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage content, ModelStateDictionary modelState)
    {
        AddQualificationNameError(model, modelState);
        AddOptionError(model, modelState, content);

        var isRadioOptionSelected = !string.IsNullOrEmpty(model.Option);
        var isRadioOptionBefore2014 = model.Option == content.BeforeSeptember2014Option.Value;

        if (isRadioOptionSelected && !isRadioOptionBefore2014)
        {
            var datesModelValidationResult = ValidateDates(model, content);

            var startedDateErrors = MapDateModel(model.RadioButtonWithDateInputModel.Question, content.AfterSeptember2014Option.StartedQuestion, datesModelValidationResult.StartedValidationResult!, "RadioButtonWithDateInputModel.Question");
            model.Errors.AddRange(startedDateErrors.ErrorSummaryLinks);

            var awardedDateErrors = MapDateModel(model.AwardedDate, content.AwardedDateQuestion, datesModelValidationResult.AwardedValidationResult!, "AwardedDate");
            model.Errors.AddRange(awardedDateErrors.ErrorSummaryLinks);
        }
        else
        {
            modelState.Remove("RadioButtonWithDateInputModel.Question.SelectedMonth");
            modelState.Remove("RadioButtonWithDateInputModel.Question.SelectedYear");

            var awardedDateValidationResult = DateIsValid(model.AwardedDate, content.AwardedDateQuestion);

            var awardedDateErrors = MapDateModel(model.AwardedDate, content.AwardedDateQuestion, awardedDateValidationResult, "AwardedDate");
            model.Errors.AddRange(awardedDateErrors.ErrorSummaryLinks);
        }

        AddAwardingOrganisationError(model, modelState);
    }

    private static void AddQualificationNameError(QualificationDetailsPageViewModel model, ModelStateDictionary modelState)
    {
        model.HasQualificationNameError = modelState.Keys.Any(_ => modelState["QualificationName"]?.Errors.Count > 0);
        if (model.HasQualificationNameError)
        {
            model.Errors.Add(
                new ErrorSummaryLink
                {
                    ErrorBannerLinkText = model.QualificationNameErrorMessage,
                    ElementLinkId = "QualificationName"
                }
            );
        }
    }

    private static void AddOptionError(QualificationDetailsPageViewModel model, ModelStateDictionary modelState, HelpQualificationDetailsPage content)
    {
        model.HasOptionError = modelState.Keys.Any(_ => modelState["Option"]?.Errors.Count > 0);
        if (model.HasOptionError)
        {
            model.Errors.Add(
                new ErrorSummaryLink
                {
                    ErrorBannerLinkText = model.MissingStartedDateOptionErrorMessage,
                    ElementLinkId = model.Before2014Option.Value
                }
            );
        }
    }

    private static void AddAwardingOrganisationError(QualificationDetailsPageViewModel model, ModelStateDictionary modelState)
    {
        model.HasAwardingOrganisationError = modelState.Keys.Any(_ => modelState["AwardingOrganisation"]?.Errors.Count > 0);
        if (model.HasAwardingOrganisationError)
        {
            model.Errors.Add(
                new ErrorSummaryLink
                {
                    ErrorBannerLinkText = model.AwardingOrganisationErrorMessage,
                    ElementLinkId = "AwardingOrganisation"
                }
            );
        }
    }
}
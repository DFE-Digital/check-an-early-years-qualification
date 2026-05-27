using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public interface IHelpService
{
    public bool SelectedOptionIsValid(List<Option> options, string value);

    public void SetHelpFormEnquiryReason(string selectedOption);

    public string GetWhyAreYouContactingUsSelectedOption();

    public string GetWhatDoYouWantToDoNextSelectedOption();

    public Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPageAsync();

    public Task<RadioQuestionHelpPage?> GetRadioQuestionHelpPageAsync(string entryId);

    public Task<RadioQuestionHelpPageViewModel> MapRadioQuestionHelpPageContentToViewModelAsync(RadioQuestionHelpPage content);

    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content);

    public QualificationDetailsPageViewModel MapHelpQualificationDetailsPageContentToViewModel(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content);

    public void SetHelpQualificationDetailsInCookie(QualificationDetailsPageViewModel viewModel);

    public DatesValidationResult ValidateDates(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage content);

    public Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage();

    public ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage content, string reasonForEnquiring);

    public Task<HelpEmailAddressPage?> GetHelpEmailAddressPage();

    public EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content);

    public void SendHelpPageNotification(HelpPageNotification helpPageNotification);

    public Task<HelpConfirmationPage?> GetHelpConfirmationPage();

    public Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage content);

    public HelpFormEnquiry GetHelpFormEnquiry();

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry);

    public Task<StaticPage?> GetStaticPage(string entryId);

    public Task<StaticPageModel?> MapStaticPage(StaticPage entryId);

    public DateValidationResult DateIsValid(DateQuestionModel model, DateQuestion content);

    public DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question, DateValidationResult validationResult, string objectName);

    public void AddQualificationDetailsValidationErrors(QualificationDetailsPageViewModel model, HelpQualificationDetailsPage content, ModelStateDictionary modelState);
}
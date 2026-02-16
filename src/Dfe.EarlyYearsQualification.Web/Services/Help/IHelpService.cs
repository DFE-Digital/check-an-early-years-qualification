using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Entities.Help;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.HelpViewModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.Notifications;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public interface IHelpService
{
    public Task<GetHelpPage?> GetGetHelpPageAsync();

    Task<GetHelpPageViewModel> MapGetHelpPageContentToViewModelAsync(GetHelpPage content);

    public bool SelectedOptionIsValid(List<EnquiryOption> options, string value);

    public bool SelectedOptionIsValid(List<Option> options, string value);

    RedirectToActionResult SetHelpFormEnquiryReason(GetHelpPageViewModel model);

    public string GetWhyAreYouContactingUsSelectedOption();

    public string GetWhatDoYouWantToDoNextSelectedOption();

    public Task<HelpQualificationDetailsPage?> GetHelpQualificationDetailsPageAsync();

    public Task<HelpProceedWithQualificationQueryPage?> GetProceedWithQualificationQueryPageAsync();

    public Task<ProceedWithQualificationQueryViewModel> MapProceedWithQualificationQueryPageContentToViewModelAsync(HelpProceedWithQualificationQueryPage content);

    public void SetAnyPreviouslyEnteredQualificationDetailsFromCookie(QualificationDetailsPageViewModel viewModel);

    public QualificationDetailsPageViewModel MapHelpQualificationDetailsPageContentToViewModel(QualificationDetailsPageViewModel viewModel, HelpQualificationDetailsPage content, DatesValidationResult? validationResult, ModelStateDictionary modelState);

    public void SetHelpQualificationDetailsInCookie(HelpFormEnquiry formEnquiry, QualificationDetailsPageViewModel viewModel);

    DatesValidationResult ValidateDates(DatesQuestionModel questionModel, HelpQualificationDetailsPage content);

    public bool HasInvalidDates(DatesValidationResult datesValidationResult);

    public Task<HelpProvideDetailsPage?> GetHelpProvideDetailsPage();

    ProvideDetailsPageViewModel MapProvideDetailsPageContentToViewModel(HelpProvideDetailsPage content, string reasonForEnquiring);

    public Task<HelpEmailAddressPage?> GetHelpEmailAddressPage();

    public EmailAddressPageViewModel MapEmailAddressPageContentToViewModel(HelpEmailAddressPage content);

    void SendHelpPageNotification(HelpPageNotification helpPageNotification);

    public Task<HelpConfirmationPage?> GetHelpConfirmationPage();

    public Task<ConfirmationPageViewModel> MapConfirmationPageContentToViewModelAsync(HelpConfirmationPage content);

    public HelpFormEnquiry GetHelpFormEnquiry();

    public void SetHelpFormEnquiry(HelpFormEnquiry formEnquiry);

    public Task<StaticPage?> GetStaticPage(string entryId);

    public Task<StaticPageModel?> MapStaticPage(StaticPage entryId);
}
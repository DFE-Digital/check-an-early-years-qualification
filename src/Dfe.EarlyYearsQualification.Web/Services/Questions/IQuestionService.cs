using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public interface IQuestionService
{
    public void ResetUserJourneyCookie();

    public string GetIsUserCheckingTheirOwnQualification();

    public void SetIsUserCheckingTheirOwnQualification(string option);

    public int? GetLevelOfQualification();

    public string? GetWhereWasQualificationAwarded();

    public string? GetAwardingOrganisation();

    public bool GetAwardingOrganisationIsNotOnList();

    public Task<RadioQuestionPage?> GetRadioQuestionPageContent(string questionPage);

    public IActionResult RedirectBasedOnQualificationLevelSelected(string option);

    public IActionResult RedirectBasedOnWhereTheQualificationWasAwarded(string option);

    public Task<RadioQuestionModel> Map(RadioQuestionModel model, RadioQuestionPage questionPage, string actionName, string controllerName, string? selectedAnswer);

    public Task<IActionResult> GetPreCheckView();

    public Task<PreCheckPageModel> MapPreCheckModel(PreCheckPage preCheckPage);

    Task<PreCheckPage?> GetPreCheckPage();

    public Task<List<Qualification>> GetFilteredQualifications();

    public Task<DropdownQuestionModel> MapDropdownModel(DropdownQuestionModel model,
                                                       DropdownQuestionPage question,
                                                       List<Qualification> qualifications, string actionName,
                                                       string controllerName,
                                                       string? selectedAwardingOrganisation,
                                                       bool selectedNotOnTheList);

    public Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId);

    public void SetWhatIsTheAwardingOrganisationValuesInCookie(DropdownQuestionModel dropdownQuestionModel);

    public DatesQuestionModel MapDatesModel(DatesQuestionModel model, DatesQuestionPage question, string actionName,
                                             string controllerName, DatesValidationResult? validationResult);

    Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId);

    DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage);

    public void SetWhenWasQualificationStarted(DateQuestionModel model);

    public void SetWhenWasQualificationAwarded(DateQuestionModel model);
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Controllers.Questions;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Services.Help;

public class QuestionService(
    ILogger<QuestionsController> logger,
    IContentService contentService,
    IUserJourneyCookieService userJourneyCookieService,
    IQualificationsRepository repository,
    IDateQuestionModelValidator questionModelValidator,
    IPlaceholderUpdater placeholderUpdater,
    IRadioQuestionMapper radioQuestionMapper,
    IDropdownQuestionMapper dropdownQuestionMapper,
    IPreCheckPageMapper preCheckPageMapper
) : ServiceController, IQuestionService
{
    public async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName,
                                                   string? selectedAnswer)
    {
        var questionPage = await GetRadioQuestionPageContent(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await radioQuestionMapper.Map(new RadioQuestionModel(), questionPage, actionName, controllerName,
                                        selectedAnswer);

        return View("Radio", model);
    }

    public async Task<RadioQuestionPage?> GetRadioQuestionPageContent(string questionPage)
    {
        return await contentService.GetRadioQuestionPage(questionPage);
    }

    public void ResetUserJourneyCookie()
    {
        userJourneyCookieService.ResetUserJourneyCookie();
    }

    public string GetIsUserCheckingTheirOwnQualification()
    {
        return userJourneyCookieService.GetIsUserCheckingTheirOwnQualification();
    }

    public void SetIsUserCheckingTheirOwnQualification(string option)
    {
        userJourneyCookieService.SetIsUserCheckingTheirOwnQualification(option);
    }

    public int? GetLevelOfQualification()
    {
        return userJourneyCookieService.GetLevelOfQualification();
    }

    public string? GetWhereWasQualificationAwarded()
    {
        return userJourneyCookieService.GetWhereWasQualificationAwarded();
    }

    public string? GetAwardingOrganisation()
    {
        return userJourneyCookieService.GetAwardingOrganisation();
    }
    public bool GetAwardingOrganisationIsNotOnList()
    {
        return userJourneyCookieService.GetAwardingOrganisationIsNotOnList();
    }

    public async Task<RadioQuestionModel> Map(RadioQuestionModel model, RadioQuestionPage questionPage, string actionName, string controllerName, string? selectedAnswer)
    {
        return await radioQuestionMapper.Map(model, questionPage, actionName, controllerName, model.Option);
    }

    public IActionResult RedirectBasedOnQualificationLevelSelected(string option)
    {
        userJourneyCookieService.SetLevelOfQualification(option);

        return option switch
        {
            "2" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                RedirectToAction("QualificationsStartedBetweenSept2014AndAug2019", "Advice"),
            "7" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                RedirectToAction(nameof(AdviceController.Level7QualificationStartedBetweenSept2014AndAug2019),
                                 "Advice"),
            "7" when userJourneyCookieService.WasStartedOnOrAfterSeptember2019() =>
                RedirectToAction(nameof(AdviceController.Level7QualificationAfterAug2019), "Advice"),
            _ => RedirectToAction(nameof(QuestionsController.WhatIsTheAwardingOrganisation))
        };
    }

    public IActionResult RedirectBasedOnWhereTheQualificationWasAwarded(string option)
    {
        userJourneyCookieService.SetWhereWasQualificationAwarded(option);

        return option switch
        {
            QualificationAwardLocation.OutsideOfTheUnitedKingdom =>
                RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice"),
            QualificationAwardLocation.Scotland =>
                RedirectToAction("QualificationsAchievedInScotland", "Advice"),
            QualificationAwardLocation.Wales => RedirectToAction("QualificationsAchievedInWales", "Advice"),
            QualificationAwardLocation.NorthernIreland =>
                RedirectToAction("QualificationsAchievedInNorthernIreland", "Advice"),
            _ => RedirectToAction(nameof(QuestionsController.WhenWasTheQualificationStarted))
        };
    }

    public async Task<IActionResult> GetPreCheckView()
    {
        var preCheckPage = await contentService.GetPreCheckPage();
        if (preCheckPage is null)
        {
            logger.LogError("No content for the pre-check page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapPreCheckModel(preCheckPage);

        return View("PreCheck", model);
    }

    public async Task<PreCheckPageModel> MapPreCheckModel(PreCheckPage preCheckPage)
    {
        return await preCheckPageMapper.Map(preCheckPage);
    }

    public async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        return await repository.Get(level, startDateMonth, startDateYear, null, null);
    }

    public async Task<DropdownQuestionModel> MapDropdownModel(DropdownQuestionModel model,
                                                               DropdownQuestionPage questionPage,
                                                               List<Qualification> qualifications, 
                                                               string actionName,
                                                               string controllerName,
                                                               string? selectedAwardingOrganisation,
                                                               bool selectedNotOnTheList)
    {
        string[] awardingOrganisationExclusions =
            [AwardingOrganisations.AllHigherEducation, AwardingOrganisations.Various];

        var uniqueAwardingOrganisations
            = qualifications.Select(x => x.AwardingOrganisationTitle)
                            .Distinct()
                            .Where(x => !Array.Exists(awardingOrganisationExclusions, x.Contains))
                            .Order();

        return await dropdownQuestionMapper.Map(model, questionPage, actionName, controllerName, uniqueAwardingOrganisations, selectedAwardingOrganisation,
                                                selectedNotOnTheList);
    }

    public async Task<DropdownQuestionPage?> GetDropdownQuestionPage(string entryId)
    {
        return await contentService.GetDropdownQuestionPage(entryId);
    }

    public void SetWhatIsTheAwardingOrganisationValuesInCookie(DropdownQuestionModel dropdownQuestionModel)
    {
        userJourneyCookieService.SetQualificationNameSearchCriteria(string.Empty);
        userJourneyCookieService.SetAwardingOrganisation(dropdownQuestionModel.NotInTheList ? string.Empty : dropdownQuestionModel.SelectedValue!);
        userJourneyCookieService.SetAwardingOrganisationNotOnList(dropdownQuestionModel.NotInTheList);

        // Used to prepopulate help form
        var enquiry = userJourneyCookieService.GetHelpFormEnquiry();
        enquiry.AwardingOrganisation = dropdownQuestionModel.SelectedValue ?? "";
        userJourneyCookieService.SetHelpFormEnquiry(enquiry);
    }

    public async Task<PreCheckPage?> GetPreCheckPage()
    {
        return await contentService.GetPreCheckPage();
    }

    public DatesQuestionModel MapDatesModel(DatesQuestionModel model, DatesQuestionPage questionPage, string actionName,
                                             string controllerName, DatesValidationResult? validationResult)
    {
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var startedModel = MapDateModel(new DateQuestionModel(), questionPage.StartedQuestion!,
                                        validationResult?.StartedValidationResult, startMonth, startYear);

        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardedModel = MapDateModel(new DateQuestionModel(), questionPage.AwardedQuestion!,
                                        validationResult?.AwardedValidationResult, awardedMonth, awardedYear);

        return DatesQuestionMapper.Map(model, questionPage, actionName, controllerName, startedModel, awardedModel);
    }

    public async Task<DatesQuestionPage?> GetDatesQuestionPage(string entryId)
    {
        return await contentService.GetDatesQuestionPage(entryId);
    }

    public DatesValidationResult IsValid(DatesQuestionModel model, DatesQuestionPage questionPage)
    {
        return questionModelValidator.IsValid(model, questionPage!);
    }

    public void SetWhenWasQualificationStarted(DateQuestionModel question)
    {
        userJourneyCookieService.SetWhenWasQualificationStarted(question!.SelectedMonth.ToString() + '/' +
                                                                    question.SelectedYear);
    }

    public void SetWhenWasQualificationAwarded(DateQuestionModel question)
    {
        userJourneyCookieService.SetWhenWasQualificationAwarded(question!.SelectedMonth.ToString() + '/' +
                                                                question.SelectedYear);
    }

    private DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question,
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
            errorBannerMessages.Add(new BannerError(question.ErrorMessage, FieldId.Month));
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
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions")]
public class QuestionsController(
    ILogger<QuestionsController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService,
    IQualificationsRepository repository,
    IDateQuestionModelValidator questionModelValidator,
    IPlaceholderUpdater placeholderUpdater)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("start-new")]
    [ResponseCache(NoStore = true)]
    public IActionResult StartNew()
    {
        userJourneyCookieService.ResetUserJourneyCookie();
        return RedirectToAction(nameof(this.WhereWasTheQualificationAwarded));
    }

    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        return await GetRadioView(QuestionPages.WhereWasTheQualificationAwarded,
                                  nameof(this.WhereWasTheQualificationAwarded),
                                  Questions, userJourneyCookieService.GetWhereWasQualificationAwarded());
    }

    [HttpPost("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded(RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhereWasTheQualificationAwarded);

            // ReSharper disable once InvertIf
            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhereWasTheQualificationAwarded),
                                            Questions, model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetWhereWasQualificationAwarded(model.Option);

        return model.Option switch
               {
                   QualificationAwardLocation.OutsideOfTheUnitedKingdom =>
                       RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice"),
                   QualificationAwardLocation.Scotland =>
                       RedirectToAction("QualificationsAchievedInScotland", "Advice"),
                   QualificationAwardLocation.Wales => RedirectToAction("QualificationsAchievedInWales", "Advice"),
                   QualificationAwardLocation.NorthernIreland =>
                       RedirectToAction("QualificationsAchievedInNorthernIreland", "Advice"),
                   _ => RedirectToAction(nameof(this.WhenWasTheQualificationStarted))
               };
    }

    [HttpGet("when-was-the-qualification-started-and-awarded")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        var questionPage = await contentService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = MapDatesModel(new DatesQuestionModel(), questionPage,
                                  nameof(this.WhenWasTheQualificationStarted),
                                  Questions,
                                  null
                                 );

        return View("Dates", model);
    }

    [HttpPost("when-was-the-qualification-started-and-awarded")]
#pragma warning disable S6967
    public async Task<IActionResult> WhenWasTheQualificationStarted(DatesQuestionModel model)
#pragma warning restore S6967
    {
        var questionPage = await contentService.GetDatesQuestionPage(QuestionPages.WhenWasTheQualificationStartedAndAwarded);
        var dateModelValidationResult = questionModelValidator.IsValid(model, questionPage!);
        if (!dateModelValidationResult.Valid)
        {
            model.HasErrors = true;
            // ReSharper disable once InvertIf
            if (questionPage is not null)
            {
                model = MapDatesModel(model, questionPage, nameof(this.WhenWasTheQualificationStarted), Questions, dateModelValidationResult);
            }

            return View("Dates", model);
        }

        userJourneyCookieService.SetWhenWasQualificationStarted(model.StartedQuestion.SelectedMonth.ToString() + '/' + model.StartedQuestion.SelectedYear);
        userJourneyCookieService.SetWhenWasQualificationAwarded(model.AwardedQuestion.SelectedMonth.ToString() + '/' + model.AwardedQuestion.SelectedYear);

        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    [RedirectIfDateMissing]
    [HttpGet("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification()
    {
        return await GetRadioView(QuestionPages.WhatLevelIsTheQualification, nameof(this.WhatLevelIsTheQualification),
                                  Questions, userJourneyCookieService.GetLevelOfQualification()?.ToString());
    }

    [RedirectIfDateMissing]
    [HttpPost("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification(RadioQuestionModel model)
    {
        if (!ModelState.IsValid)
        {
            var questionPage = await contentService.GetRadioQuestionPage(QuestionPages.WhatLevelIsTheQualification);

            // ReSharper disable once InvertIf
            if (questionPage is not null)
            {
                model = await MapRadioModel(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions,
                                            model.Option);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetLevelOfQualification(model.Option);

        return model.Option switch
               {
                   "2" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                       RedirectToAction("QualificationsStartedBetweenSept2014AndAug2019", "Advice"),
                   "7" when userJourneyCookieService.WasStartedBetweenSeptember2014AndAugust2019() =>
                       RedirectToAction(nameof(AdviceController.Level7QualificationStartedBetweenSept2014AndAug2019), "Advice"),
                   "7" when userJourneyCookieService.WasStartedOnOrAfterSeptember2019() =>
                       RedirectToAction(nameof(AdviceController.Level7QualificationAfterAug2019), "Advice"),
                   _ => RedirectToAction(nameof(this.WhatIsTheAwardingOrganisation))
               };
    }

    [RedirectIfDateMissing]
    [HttpGet("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation()
    {
        var questionPage = await contentService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var qualifications = await GetFilteredQualifications();

        var model = await MapDropdownModel(new DropdownQuestionModel(), questionPage, qualifications,
                                           nameof(this.WhatIsTheAwardingOrganisation),
                                           Questions,
                                           userJourneyCookieService.GetAwardingOrganisation(),
                                           userJourneyCookieService.GetAwardingOrganisationIsNotOnList());

        return View("Dropdown", model);
    }

    [RedirectIfDateMissing]
    [HttpPost("what-is-the-awarding-organisation")]
    public async Task<IActionResult> WhatIsTheAwardingOrganisation(DropdownQuestionModel model)
    {
        if (!ModelState.IsValid || (string.IsNullOrEmpty(model.SelectedValue) && !model.NotInTheList))
        {
            var questionPage =
                await contentService.GetDropdownQuestionPage(QuestionPages.WhatIsTheAwardingOrganisation);

            // ReSharper disable once InvertIf
            if (questionPage is not null)
            {
                var qualifications = await GetFilteredQualifications();

                model = await MapDropdownModel(model, questionPage, qualifications,
                                               nameof(this.WhatIsTheAwardingOrganisation),
                                               Questions,
                                               model.SelectedValue,
                                               model.NotInTheList);
                model.HasErrors = true;
            }

            return View("Dropdown", model);
        }

        userJourneyCookieService.SetQualificationNameSearchCriteria(string.Empty);
        userJourneyCookieService.SetAwardingOrganisation(model.NotInTheList ? string.Empty : model.SelectedValue!);
        userJourneyCookieService.SetAwardingOrganisationNotOnList(model.NotInTheList);

        return RedirectToAction("Get", "QualificationSearch");
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        return await repository.Get(level, startDateMonth, startDateYear, null, null);
    }

    private async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName,
                                                   string? selectedAnswer)
    {
        var questionPage = await contentService.GetRadioQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapRadioModel(new RadioQuestionModel(), questionPage, actionName, controllerName,
                                        selectedAnswer);

        return View("Radio", model);
    }

    private async Task<RadioQuestionModel> MapRadioModel(RadioQuestionModel model, RadioQuestionPage question,
                                                         string actionName,
                                                         string controllerName,
                                                         string? selectedAnswer)
    {
        var additionalInformationBody = await contentParser.ToHtml(question.AdditionalInformationBody);
        return RadioQuestionMapper.Map(model, question, actionName, controllerName, additionalInformationBody,
                                       selectedAnswer);
    }

    private DateQuestionModel MapDateModel(DateQuestionModel model, DateQuestion question,
                                           DateValidationResult? validationResult,
                                           int? selectedMonth,
                                           int? selectedYear)
    {
        var bannerErrorText = validationResult is { BannerErrorMessages.Count: > 0 }
                                  ? string.Join("<br />", validationResult!.BannerErrorMessages)
                                  : null;

        var errorMessageText = validationResult is { ErrorMessages.Count: > 0 }
                                   ? string.Join("<br />", validationResult!.ErrorMessages)
                                   : null;

        var errorBannerLinkText = placeholderUpdater.Replace(bannerErrorText ?? question.ErrorBannerLinkText);

        var errorMessage = placeholderUpdater.Replace(errorMessageText ?? question.ErrorMessage);

        return DateQuestionMapper.Map(model, question, errorBannerLinkText, errorMessage, validationResult, selectedMonth, selectedYear);
    }

    private DatesQuestionModel MapDatesModel(DatesQuestionModel model, DatesQuestionPage question, string actionName, string controllerName, DatesValidationResult? validationResult)
    {
        var (startMonth, startYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var startedModel = MapDateModel(new DateQuestionModel(), question.StartedQuestion, validationResult?.StartedValidationResult, startMonth, startYear);

        var (awardedMonth, awardedYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardedModel = MapDateModel(new DateQuestionModel(), question.AwardedQuestion, validationResult?.AwardedValidationResult, awardedMonth, awardedYear);

        return DatesQuestionMapper.Map(model, question, actionName, controllerName, startedModel, awardedModel);
    }

    private async Task<DropdownQuestionModel> MapDropdownModel(DropdownQuestionModel model,
                                                               DropdownQuestionPage question,
                                                               List<Qualification> qualifications, string actionName,
                                                               string controllerName,
                                                               string? selectedAwardingOrganisation,
                                                               bool selectedNotOnTheList)
    {
        var awardingOrganisationExclusions =
            new[] { AwardingOrganisations.AllHigherEducation, AwardingOrganisations.Various };

        var uniqueAwardingOrganisations
            = qualifications.Select(x => x.AwardingOrganisationTitle)
                            .Distinct()
                            .Where(x => !Array.Exists(awardingOrganisationExclusions, x.Contains))
                            .Order();

        var additionalInformationBodyHtml = await contentParser.ToHtml(question.AdditionalInformationBody);

        return DropdownQuestionMapper.Map(model, question, actionName, controllerName, uniqueAwardingOrganisations,
                                          additionalInformationBodyHtml, selectedAwardingOrganisation,
                                          selectedNotOnTheList);
    }
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels.Validators;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/questions")]
public class QuestionsController(
    ILogger<QuestionsController> logger,
    IContentService contentService,
    IHtmlRenderer renderer,
    IUserJourneyCookieService userJourneyCookieService,
    IContentFilterService contentFilterService,
    IDateQuestionModelValidator questionModelValidator)
    : ServiceController
{
    private const string Questions = "Questions";

    [HttpGet("where-was-the-qualification-awarded")]
    public async Task<IActionResult> WhereWasTheQualificationAwarded()
    {
        userJourneyCookieService.ResetUserJourneyCookie();
        return await GetRadioView(QuestionPages.WhereWasTheQualificationAwarded,
                                  nameof(this.WhereWasTheQualificationAwarded),
                                  Questions);
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
                                            Questions);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        switch (model.Option)
        {
            case QualificationAwardLocation.OutsideOfTheUnitedKingdom:
                return RedirectToAction("QualificationOutsideTheUnitedKingdom", "Advice");
            case QualificationAwardLocation.Scotland:
                return RedirectToAction("QualificationsAchievedInScotland", "Advice");
            case QualificationAwardLocation.Wales:
                return RedirectToAction("QualificationsAchievedInWales", "Advice");
            case QualificationAwardLocation.NorthernIreland:
                return RedirectToAction("QualificationsAchievedInNorthernIreland", "Advice");
        }

        userJourneyCookieService.SetWhereWasQualificationAwarded(model.Option!);

        return RedirectToAction(nameof(this.WhenWasTheQualificationStarted));
    }

    [HttpGet("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted()
    {
        // This is just a temporary page until the design is finalised through UR
        var questionPage = await contentService.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapDateModel(new DateQuestionModel(), questionPage,
                                       nameof(this.WhenWasTheQualificationStarted),
                                       Questions);
        return View("Date", model);
    }

    [HttpPost("when-was-the-qualification-started")]
    public async Task<IActionResult> WhenWasTheQualificationStarted(DateQuestionModel model)
    {
        if (!ModelState.IsValid || !questionModelValidator.IsValid(model))
        {
            var questionPage = await contentService.GetDateQuestionPage(QuestionPages.WhenWasTheQualificationStarted);

            // ReSharper disable once InvertIf
            if (questionPage is not null)
            {
                model = await MapDateModel(model, questionPage, nameof(this.WhenWasTheQualificationStarted), Questions);
                model.HasErrors = true;
            }

            return View("Date", model);
        }

        userJourneyCookieService.SetWhenWasQualificationAwarded(model.SelectedMonth.ToString() + '/' +
                                                                model.SelectedYear);

        return RedirectToAction(nameof(this.WhatLevelIsTheQualification));
    }

    [RedirectIfDateMissing]
    [HttpGet("what-level-is-the-qualification")]
    public async Task<IActionResult> WhatLevelIsTheQualification()
    {
        return await GetRadioView(QuestionPages.WhatLevelIsTheQualification, nameof(this.WhatLevelIsTheQualification),
                                  Questions);
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
                model = await MapRadioModel(model, questionPage, nameof(this.WhatLevelIsTheQualification), Questions);
                model.HasErrors = true;
            }

            return View("Radio", model);
        }

        userJourneyCookieService.SetLevelOfQualification(model.Option!);
        
        return model.Option switch
               {
                   "2" when WasAwardedBetweenSeptember2014AndAugust2019() =>
                       RedirectToAction("QualificationsStartedBetweenSept2014AndAug2019", "Advice"),
                   "6" => 
                       RedirectToAction(WasStartedBeforeSeptember2014() ? "Level6QualificationPre2014" : "Level6QualificationPost2014", "Advice"),
                   "7" => RedirectToAction(nameof(AdviceController.QualificationLevel7), "Advice"),
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
                                           Questions);

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
                                               Questions);
                model.HasErrors = true;
            }

            return View("Dropdown", model);
        }

        var cookie = userJourneyCookieService.GetUserJourneyModelFromCookie();
        cookie.SearchCriteria = string.Empty;
        cookie.WhatIsTheAwardingOrganisation = model.NotInTheList ? string.Empty : model.SelectedValue;
        userJourneyCookieService.SetUserJourneyModelCookie(cookie);

        return RedirectToAction("Get", "QualificationDetails");
    }

    private bool WasStartedBeforeSeptember2014()
    {
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();

        if (startDateMonth is null || startDateYear is null)
        {
            return false;
        }

        var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        return date < new DateOnly(2014, 9, 1);
    }
    
    private bool WasAwardedBetweenSeptember2014AndAugust2019()
    {
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();

        if (startDateMonth is null || startDateYear is null)
        {
            return false;
        }

        var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
        return date >= new DateOnly(2014, 09, 01) && date <= new DateOnly(2019, 08, 31);
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        return await contentFilterService.GetFilteredQualifications(level, startDateMonth, startDateYear, null, null);
    }

    private async Task<IActionResult> GetRadioView(string questionPageId, string actionName, string controllerName)
    {
        var questionPage = await contentService.GetRadioQuestionPage(questionPageId);
        if (questionPage is null)
        {
            logger.LogError("No content for the question page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapRadioModel(new RadioQuestionModel(), questionPage, actionName, controllerName);

        return View("Radio", model);
    }

    private async Task<RadioQuestionModel> MapRadioModel(RadioQuestionModel model, RadioQuestionPage question,
                                                         string actionName,
                                                         string controllerName)
    {
        model.Question = question.Question;
        model.OptionsItems = MapOptionItems(question.Options);
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = await renderer.ToHtml(question.AdditionalInformationBody);
        model.BackButton = MapToNavigationLinkModel(question.BackButton);
        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = question.ErrorBannerLinkText;
        return model;
    }

    private static List<IOptionItemModel> MapOptionItems(List<IOptionItem> questionOptions)
    {
        var results = new List<IOptionItemModel>();

        foreach (var optionItem in questionOptions)
        {
            if (optionItem.GetType() == typeof(Option))
            {
                var option = (Option)optionItem;
                results.Add(new OptionModel { Hint = option.Hint, Value = option.Value, Label = option.Label });
            }
            else if (optionItem.GetType() == typeof(Divider))
            {
                var divider = (Divider)optionItem;
                results.Add(new DividerModel { Text = divider.Text });
            }
        }

        return results;
    }

    private async Task<DateQuestionModel> MapDateModel(DateQuestionModel model, DateQuestionPage question,
                                                       string actionName,
                                                       string controllerName)
    {
        model.Question = question.Question;
        model.CtaButtonText = question.CtaButtonText;
        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.ErrorMessage = question.ErrorMessage;
        model.QuestionHint = question.QuestionHint;
        model.MonthLabel = question.MonthLabel;
        model.YearLabel = question.YearLabel;
        model.BackButton = MapToNavigationLinkModel(question.BackButton);
        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = question.ErrorBannerLinkText;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = await renderer.ToHtml(question.AdditionalInformationBody);
        return model;
    }

    private async Task<DropdownQuestionModel> MapDropdownModel(DropdownQuestionModel model,
                                                               DropdownQuestionPage question,
                                                               List<Qualification> qualifications, string actionName,
                                                               string controllerName)
    {
        var awardingOrganisationExclusions =
            new[] { AwardingOrganisations.AllHigherEducation, AwardingOrganisations.Various };

        var uniqueAwardingOrganisations
            = qualifications.Select(x => x.AwardingOrganisationTitle)
                            .Distinct()
                            .Where(x => !Array.Exists(awardingOrganisationExclusions, x.Contains))
                            .Order();

        model.ActionName = actionName;
        model.ControllerName = controllerName;
        model.CtaButtonText = question.CtaButtonText;
        model.ErrorMessage = question.ErrorMessage;
        model.Question = question.Question;
        model.DropdownHeading = question.DropdownHeading;
        model.NotInListText = question.NotInListText;
        model.BackButton = MapToNavigationLinkModel(question.BackButton);

        model.Values.Add(new SelectListItem
                         {
                             Text = question.DefaultText,
                             Value = ""
                         });

        foreach (var awardingOrg in uniqueAwardingOrganisations)
        {
            model.Values.Add(new SelectListItem
                             {
                                 Value = awardingOrg,
                                 Text = awardingOrg
                             });
        }

        model.ErrorBannerHeading = question.ErrorBannerHeading;
        model.ErrorBannerLinkText = question.ErrorBannerLinkText;
        model.AdditionalInformationHeader = question.AdditionalInformationHeader;
        model.AdditionalInformationBody = await renderer.ToHtml(question.AdditionalInformationBody);
        return model;
    }
}
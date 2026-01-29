using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("early-years-qualification-list")]
public class QualificationListController(
    ILogger<QualificationListController> logger, 
    IQualificationsRepository qualificationsRepository, 
    IUserJourneyCookieService userJourneyCookieService,
    IContentService contentService) : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contentModel = await contentService.GetChallengePage();

        if (contentModel == null)
        {
            logger.LogError("No content for the challenge page");
            return RedirectToAction("Index", "Error");
        }

        var model = await SetUpEarlyYearsQualificationListModel();

        return View(model);
    }

    [HttpGet("/clear-filters")]
    public IActionResult ClearFilters()
    {
        userJourneyCookieService.SetWebViewFilters(new WebViewFilters());

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("ApplyFilter")]
    public IActionResult ApplyFilter(EarlyYearsQualificationListModel model)
    {
        var filters = userJourneyCookieService.GetWebViewFilters();
        filters.SearchTerm = model.SearchTermFilter;
        filters.QualificationStartDate = model.QualificationStartDateFilter;
        filters.QualificationLevel = model.QualificationLevelFilter;
        userJourneyCookieService.SetWebViewFilters(filters);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("RemoveFilter")]
    public IActionResult RemoveFilter(string removeFilter)
    {
        var filters = userJourneyCookieService.GetWebViewFilters();

        if (removeFilter.Contains("qualification-level"))
        {
            filters.QualificationLevel = string.Empty;
        }
        if (removeFilter.Contains("start-date"))
        {
            filters.QualificationStartDate = string.Empty;
        }
        if (removeFilter.Contains("search-term"))
        {
            filters.SearchTerm = string.Empty;
        }

        userJourneyCookieService.SetWebViewFilters(filters);

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<Qualification>> GetQualifications(WebViewFilters filters)
    {
        var searchCriteria = string.IsNullOrWhiteSpace(filters.SearchTerm) ? null : filters.SearchTerm;
        var (startMonth, startYear) = GetQualificationStartDate(filters.QualificationStartDate);
        var qualificationLevel = GetQualificationLevel(filters.QualificationLevel);

        return [..(await qualificationsRepository.Get(
                                                  qualificationLevel,
                                                  startMonth,
                                                  startYear,
                                                  null,
                                                  searchCriteria
                                                 )).OrderBy(x => x.QualificationLevel)
                                                   .ThenBy(x => x.QualificationName)];
    }

    private static int GetQualificationLevel(string qualificationLevel) 
    {
        if (!string.IsNullOrWhiteSpace(qualificationLevel) && int.TryParse(qualificationLevel, out var parsedQualificationLevel))
        {
            return parsedQualificationLevel;
        }

        return 0;
    }

    private static (int?, int?) GetQualificationStartDate(string qualificationStartDate)
    {
        int? startMonth = null;
        int? startYear = null;

        // TODO check logic
        if (qualificationStartDate == "Before September 2014")
        {
            startMonth = 9;
            startYear = 2014;
        }
        if (qualificationStartDate == "On or after September 2014")
        {
            startMonth = 9;
            startYear = 2014;
        }
        if (qualificationStartDate == "On or after September 2024")
        {
            startMonth = 9;
            startYear = 2024;
        }

        return (startMonth, startYear);
    }

    private static List<QualificationWebViewModel> MapToQualificationModels(List<Qualification> allQualifications)
    {
        var results = new List<QualificationWebViewModel>();

        foreach (var qualification in allQualifications)
        {
            var qual = new QualificationWebViewModel()
            {
                QualificationId = qualification.QualificationId,
                QualificationName = qualification.QualificationName,
                AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
                QualificationLevel = qualification.QualificationLevel,
                FromWhichYear = FormatYearContent(qualification.FromWhichYear),
                ToWhichYear = FormatYearContent(qualification.ToWhichYear),
                AdditionalRequirements = string.IsNullOrEmpty(qualification.AdditionalRequirements) ? "None" : qualification.AdditionalRequirements,
                StaffChildRatio = qualification.StaffChildRatio,
                QualificationNumber = string.IsNullOrEmpty(qualification.QualificationNumber) ? "-" : StringFormattingHelper.FormatSlashedNumbers(qualification.QualificationNumber),
            };

            results.Add(qual);
        }

        return results;
    }

    private static string FormatYearContent(string? year)
    {
        var content = "-";

        if (!string.IsNullOrEmpty(year) && year is not "null")
        {
            var convertedFromDate = StringDateHelper.ConvertDate(year);
            if (convertedFromDate.HasValue)
            {
                content = StringDateHelper.ConvertToDateString(convertedFromDate.Value.startMonth, convertedFromDate.Value.startYear, "");
            }
        }

        return content;
    }

    private async Task<EarlyYearsQualificationListModel> SetUpEarlyYearsQualificationListModel()
    {
        var filters = userJourneyCookieService.GetWebViewFilters();

        var allQualifications = await GetQualifications(filters);

        var model = new EarlyYearsQualificationListModel
        {
            Heading = EarlyYearsQualificationListContent.Heading,
            PostHeadingContent = EarlyYearsQualificationListContent.PostHeadingContent,
            BackButton = new NavigationLinkModel
            {
                DisplayText = "Home",
                Href = "/",
            },
            Qualifications = MapToQualificationModels(allQualifications),
            DownloadButtonText = EarlyYearsQualificationListContent.DownloadButtonText,
            QualificationLevelLabel = EarlyYearsQualificationListContent.QualificationLevelLabel,
            StaffChildRatioLabel = EarlyYearsQualificationListContent.StaffChildRatioLabel,
            FromWhichYearLabel = EarlyYearsQualificationListContent.FromWhichYearLabel,
            ToWhichYearLabel = EarlyYearsQualificationListContent.ToWhichYearLabel,
            AwardingOrganisationLabel = EarlyYearsQualificationListContent.AwardingOrganisationLabel,
            QualificationNumberLabel = EarlyYearsQualificationListContent.QualificationNumberLabel,
            NotesAdditionalRequirementsLabel = EarlyYearsQualificationListContent.NotesAdditionalRequirementsLabel,
            QualificationLevelFilter = filters.QualificationLevel,
            QualificationStartDateFilter = filters.QualificationStartDate,
            SearchTermFilter = filters.SearchTerm,
            NoQualificationsFoundContent = EarlyYearsQualificationListContent.NoQualificationsFoundContent,
            CheckIfAnEarlyYearsQualificationIsFullAndRelevantContent = EarlyYearsQualificationListContent.CheckIfAnEarlyYearsQualificationIsFullAndRelevantContent
        };
        model.ShowingAllQualificationsLabel = model.HasFilters ? $"{allQualifications.Count} qualification{(allQualifications.Count == 1 ? "" : "s")} found" : EarlyYearsQualificationListContent.ShowingAllQualificationsLabel;

        return model;
    }
}
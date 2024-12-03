using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
[RedirectIfDateMissing]
public class QualificationSearchController(
    ILogger<QualificationSearchController> logger,
    IQualificationsRepository qualificationsRepository,
    IContentService contentService,
    IGovUkContentParser contentParser,
    IUserJourneyCookieService userJourneyCookieService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var listPageContent = await contentService.GetQualificationListPage();
        if (listPageContent is null)
        {
            logger.LogError("No content for the qualification list page");
            return RedirectToAction("Index", "Error");
        }

        var model = await MapList(listPageContent, await GetFilteredQualifications());

        return View(model);
    }

    [HttpPost]
    public IActionResult Refine(string? refineSearch)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning($"Invalid model state in {nameof(QualificationSearchController)} POST");
        }

        userJourneyCookieService.SetQualificationNameSearchCriteria(refineSearch ?? string.Empty);

        return RedirectToAction("Get");
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await qualificationsRepository.Get(level, startDateMonth, startDateYear,
                                                  awardingOrganisation, searchCriteria);
    }

    private async Task<QualificationListModel> MapList(QualificationListPage content,
                                                       List<Qualification>? qualifications)
    {
        var basicQualificationsModels = GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel(content);

        return new QualificationListModel
               {
                   BackButton = NavigationLinkMapper.Map(content.BackButton),
                   Filters = filterModel,
                   Header = content.Header,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await contentParser.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   LevelHeading = content.LevelHeading,
                   AwardingOrganisationHeading = content.AwardingOrganisationHeading,
                   PostSearchCriteriaContent = await contentParser.ToHtml(content.PostSearchCriteriaContent),
                   PostQualificationListContent = await contentParser.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels.OrderBy(x => x.QualificationName).ToList(),
                   NoResultText = await contentParser.ToHtml(content.NoResultsText),
                   ClearSearchText = content.ClearSearchText,
                   NoQualificationsFoundText = content.NoQualificationsFoundText
               };
    }

    private FilterModel GetFilterModel(QualificationListPage content)
    {
        var filterModel = new FilterModel
                          {
                              Country = userJourneyCookieService.GetWhereWasQualificationAwarded()!,
                              Level = content.AnyLevelHeading,
                              AwardingOrganisation = content.AnyAwardingOrganisationHeading
                          };

        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationStarted();
        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
            filterModel.StartDate = $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        var level = userJourneyCookieService.GetLevelOfQualification();
        if (level > 0)
        {
            filterModel.Level = $"Level {level}";
        }

        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        if (!string.IsNullOrEmpty(awardingOrganisation))
        {
            filterModel.AwardingOrganisation = awardingOrganisation;
        }

        return filterModel;
    }

    private static List<BasicQualificationModel> GetBasicQualificationsModels(List<Qualification>? qualifications)
    {
        var basicQualificationsModels = new List<BasicQualificationModel>();

        // ReSharper disable once InvertIf
        if (qualifications is not null && qualifications.Count > 0)
        {
            foreach (var qualification in qualifications)
            {
                basicQualificationsModels.Add(new BasicQualificationModel
                                              {
                                                  QualificationId = qualification.QualificationId,
                                                  QualificationLevel = qualification.QualificationLevel,
                                                  QualificationName = qualification.QualificationName,
                                                  AwardingOrganisationTitle = qualification.AwardingOrganisationTitle
                                              });
            }
        }

        return basicQualificationsModels;
    }
}
using System.Globalization;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IContentService contentService,
    IContentFilterService contentFilterService,
    IGovUkInsetTextRenderer govUkInsetTextRenderer,
    IHtmlRenderer htmlRenderer,
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
            logger.LogError($"Invalid model state in {nameof(QualificationDetailsController)} POST");
        }

        userJourneyCookieService.SetQualificationNameSearchCriteria(refineSearch ?? string.Empty);

        return RedirectToAction("Get");
    }

    [HttpGet("qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(qualificationId))
        {
            return BadRequest();
        }

        var detailsPageContent = await contentService.GetDetailsPage();
        if (detailsPageContent is null)
        {
            logger.LogError("No content for the qualification details page");
            return RedirectToAction("Index", "Error");
        }

        var qualification = await contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            var loggedQualificationId = qualificationId.Replace(Environment.NewLine, "");
            logger.LogError("Could not find details for qualification with ID: {QualificationId}",
                            loggedQualificationId);

            return RedirectToAction("Index", "Error");
        }

        var model = await MapDetails(qualification, detailsPageContent);
        return View(model);
    }

    private async Task<List<Qualification>> GetFilteredQualifications()
    {
        var level = userJourneyCookieService.GetLevelOfQualification();
        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        var awardingOrganisation = userJourneyCookieService.GetAwardingOrganisation();
        var searchCriteria = userJourneyCookieService.GetSearchCriteria();

        return await contentFilterService.GetFilteredQualifications(level, startDateMonth, startDateYear,
                                                                    awardingOrganisation, searchCriteria);
    }

    private async Task<QualificationListModel> MapList(QualificationListPage content,
                                                       List<Qualification>? qualifications)
    {
        var basicQualificationsModels = GetBasicQualificationsModels(qualifications);

        var filterModel = GetFilterModel();

        return new QualificationListModel
               {
                   BackButton = content.BackButton,
                   Filters = filterModel,
                   Header = content.Header,
                   SingleQualificationFoundText = content.SingleQualificationFoundText,
                   MultipleQualificationsFoundText = content.MultipleQualificationsFoundText,
                   PreSearchBoxContent = await htmlRenderer.ToHtml(content.PreSearchBoxContent),
                   SearchButtonText = content.SearchButtonText,
                   LevelHeading = content.LevelHeading,
                   AwardingOrganisationHeading = content.AwardingOrganisationHeading,
                   PostSearchCriteriaContent = await htmlRenderer.ToHtml(content.PostSearchCriteriaContent),
                   PostQualificationListContent = await htmlRenderer.ToHtml(content.PostQualificationListContent),
                   SearchCriteriaHeading = content.SearchCriteriaHeading,
                   SearchCriteria = userJourneyCookieService.GetSearchCriteria(),
                   Qualifications = basicQualificationsModels.OrderBy(x => x.QualificationName).ToList()
               };
    }

    private FilterModel GetFilterModel()
    {
        var filterModel = new FilterModel
                          {
                              Country = userJourneyCookieService.GetWhereWasQualificationAwarded()!
                          };

        var (startDateMonth, startDateYear) = userJourneyCookieService.GetWhenWasQualificationAwarded();
        if (startDateMonth is not null && startDateYear is not null)
        {
            var date = new DateOnly(startDateYear.Value, startDateMonth.Value, 1);
            filterModel.StartDate = $"{date.ToString("MMMM", CultureInfo.InvariantCulture)} {startDateYear.Value}";
        }

        var level = userJourneyCookieService.GetLevelOfQualification();
        if (level is not null && level > 0)
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

    private async Task<QualificationDetailsModel> MapDetails(Qualification qualification, DetailsPage content)
    {
        return new QualificationDetailsModel
               {
                   QualificationId = qualification.QualificationId,
                   QualificationLevel = qualification.QualificationLevel,
                   QualificationName = qualification.QualificationName,
                   QualificationNumber = qualification.QualificationNumber,
                   AwardingOrganisationTitle = qualification.AwardingOrganisationTitle,
                   FromWhichYear = qualification.FromWhichYear,
                   ToWhichYear = qualification.ToWhichYear,
                   AdditionalRequirements = qualification.AdditionalRequirements,
                   BookmarkUrl = HttpContext.Request.GetDisplayUrl(),
                   BackButton = content.BackButton,
                   Content = new DetailsPageModel
                             {
                                 AwardingOrgLabel = content.AwardingOrgLabel,
                                 BookmarkHeading = content.BookmarkHeading,
                                 BookmarkText = content.BookmarkText,
                                 CheckAnotherQualificationHeading = content.CheckAnotherQualificationHeading,
                                 CheckAnotherQualificationText =
                                     await govUkInsetTextRenderer.ToHtml(content.CheckAnotherQualificationText),
                                 DateAddedLabel = content.DateAddedLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 FurtherInfoHeading = content.FurtherInfoHeading,
                                 FurtherInfoText = await govUkInsetTextRenderer.ToHtml(content.FurtherInfoText),
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 QualificationNumberLabel = content.QualificationNumberLabel
                             }
               };
    }
}
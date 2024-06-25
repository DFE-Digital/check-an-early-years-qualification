using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Services.UserJourneyCookieService;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IContentService contentService,
    IGovUkInsetTextRenderer renderer,
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
        
        var filterParams = userJourneyCookieService.GetUserJourneyModelFromCookie();

        var model = MapList(listPageContent, filterParams);
        
        return View(model);
    }

    [HttpGet("qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (string.IsNullOrEmpty(qualificationId))
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

    private QualificationListModel MapList(QualificationListPage content, UserJourneyModel? filters)
    {
        return new QualificationListModel()
               {
                   BackButton = content.BackButton,
                   Filters = filters,
                   Header = content.Header
               };
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
                   Notes = qualification.Notes,
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
                                     await renderer.ToHtml(content.CheckAnotherQualificationText),
                                 DateAddedLabel = content.DateAddedLabel,
                                 DateOfCheckLabel = content.DateOfCheckLabel,
                                 FurtherInfoHeading = content.FurtherInfoHeading,
                                 FurtherInfoText = await renderer.ToHtml(content.FurtherInfoText),
                                 LevelLabel = content.LevelLabel,
                                 MainHeader = content.MainHeader,
                                 QualificationNumberLabel = content.QualificationNumberLabel
                             }
               };
    }
}
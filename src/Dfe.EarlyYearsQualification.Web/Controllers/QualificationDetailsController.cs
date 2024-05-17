using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController(
    ILogger<QualificationDetailsController> logger,
    IContentService contentService,
    IGovUkInsetTextRenderer renderer)
    : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return View();
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
            return RedirectToAction("Error", "Home");
        }

        var qualification = await contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = await Map(qualification, detailsPageContent);
        return View(model);
    }

    private async Task<QualificationDetailsModel> Map(Qualification qualification, DetailsPage content)
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
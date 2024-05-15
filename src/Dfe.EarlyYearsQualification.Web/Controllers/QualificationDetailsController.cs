using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.AspNetCore.Http.Extensions;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController : Controller
{
    private readonly ILogger<QualificationDetailsController> _logger;
    private readonly IContentService _contentService;
    private readonly IGovUkInsetTextRenderer _renderer;

    public QualificationDetailsController(ILogger<QualificationDetailsController> logger, IContentService contentService, IGovUkInsetTextRenderer renderer)
    {
        _logger = logger;
        _contentService = contentService;
        _renderer = renderer;
    }

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

        var detailsPageContent = await _contentService.GetDetailsPage();
        if (detailsPageContent is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var qualification = await _contentService.GetQualificationById(qualificationId);
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
            Content = new DetailsPageModel()
            {
              AwardingOrgLabel = content.AwardingOrgLabel,
              BookmarkHeading = content.BookmarkHeading,
              BookmarkText = content.BookmarkText,
              CheckAnotherQualificationHeading = content.CheckAnotherQualificationHeading,
              CheckAnotherQualificationText = await _renderer.ToHtml(content.CheckAnotherQualificationText),
              DateAddedLabel = content.DateAddedLabel,
              DateOfCheckLabel = content.DateOfCheckLabel,
              FurtherInfoHeading = content.FurtherInfoHeading,
              FurtherInfoText = await _renderer.ToHtml(content.FurtherInfoText),
              LevelLabel = content.LevelLabel,
              MainHeader = content.MainHeader,
              QualificationNumberLabel = content.QualificationNumberLabel
            },
        };
    }
}

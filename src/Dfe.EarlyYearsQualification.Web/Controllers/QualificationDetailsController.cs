using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.AspNetCore.Http.Extensions;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
public class QualificationDetailsController : Controller
{
    private readonly ILogger<QualificationDetailsController> _logger;
    private readonly IContentService _contentService;

    public QualificationDetailsController(ILogger<QualificationDetailsController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return View();
    }

    [HttpGet("qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        if (string.IsNullOrEmpty(qualificationId)) return BadRequest();

        var detailsPageContent = await _contentService.GetDetailsPage();
        if (detailsPageContent is null) return RedirectToAction("Error");

        var qualification = await _contentService.GetQualificationById(qualificationId);
        if (qualification is null)
        {
            return RedirectToAction("Error", "Home");
        }
        var model = Map(qualification);
        model.Content = detailsPageContent;
        return View(model);
    }

    private QualificationDetailsModel Map(Qualification qualification)
    {
        return new QualificationDetailsModel()
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
            BookmarkUrl = HttpContext.Request.GetDisplayUrl()
        };
    }
}

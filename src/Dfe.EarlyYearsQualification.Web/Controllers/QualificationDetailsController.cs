using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class QualificationDetailsController : Controller
{
    private readonly ILogger<QualificationDetailsController> _logger;
    private readonly IContentService _contentService;

    public QualificationDetailsController(ILogger<QualificationDetailsController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet("/qualification-details/{qualificationId}")]
    public async Task<IActionResult> Index(string qualificationId)
    {
        var qualification = await _contentService.GetQualification(qualificationId);
        var model = Map(qualification);
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
            AdditionalRequirements = qualification.AdditionalRequirements
        };
    }
}

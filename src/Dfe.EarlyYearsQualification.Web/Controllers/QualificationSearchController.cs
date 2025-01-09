using Dfe.EarlyYearsQualification.Web.Attributes;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Services.QualificationSearch;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/qualifications")]
[RedirectIfDateMissing]
public class QualificationSearchController(
    ILogger<QualificationSearchController> logger,
    IQualificationSearchService qualificationSearchService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var qualifications = await qualificationSearchService.GetQualifications();
        if (qualifications is null)
        {
            logger.LogError("No content for the qualification list page");
            return RedirectToAction("Index", "Error");
        }

        return View(qualifications);
    }

    [HttpPost]
    public IActionResult Refine(string? refineSearch)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning($"Invalid model state in {nameof(QualificationSearchController)} POST");
        }

        qualificationSearchService.Refine(refineSearch ?? string.Empty);

        return RedirectToAction("Get");
    }
}
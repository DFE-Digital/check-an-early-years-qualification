using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController(
    ILogger<AccessibilityStatementController> logger,
    IContentService contentService,
    IAccessibilityStatementMapper accessibilityStatementMapper)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await contentService.GetAccessibilityStatementPage();

        if (content is null)
        {
            logger.LogError("No content for the accessibility statement page");
            return RedirectToAction("Index", "Error");
        }
        
        var model = await accessibilityStatementMapper.Map(content);

        return View(model);
    }
}
using Dfe.EarlyYearsQualification.Content.Constants;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Constants;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class AccessibilityStatementController(
    ILogger<AccessibilityStatementController> logger,
    IContentService contentService,
    IAccessibilityStatementMapper accessibilityStatementMapper)
    : ServiceController
{
    [Route("accessibility-statement")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await contentService.GetAccessibilityStatementPage(AccessibilityStatements.Service);

        if (content is null)
        {
            logger.LogError("No content for the accessibility statement page");
            return RedirectToAction("Index", "Error");
        }
        
        var model = await accessibilityStatementMapper.Map(content);

        return View(model);
    }

    [Route(Urls.AccessibilityStatementEYQL)]
    [HttpGet]
    public async Task<IActionResult> AccessibilityStatementEYQL()
    {
        var content = await contentService.GetAccessibilityStatementPage(AccessibilityStatements.EYQL);

        if (content is null)
        {
            logger.LogError("No content for the EYQL accessibility statement page");
            return RedirectToAction("Index", "Error");
        }

        var model = await accessibilityStatementMapper.Map(content);

        return View("Index", model);
    }
}
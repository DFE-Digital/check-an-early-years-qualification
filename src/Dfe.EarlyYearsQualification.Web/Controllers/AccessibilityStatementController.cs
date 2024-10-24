using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController(
    ILogger<AccessibilityStatementController> logger,
    IContentService contentService,
    IGovUkContentParser contentParser)
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

        var bodyHtml = await contentParser.ToHtml(content.Body);
        var model = AccessibilityStatementMapper.Map(content, bodyHtml);

        return View(model);
    }
}
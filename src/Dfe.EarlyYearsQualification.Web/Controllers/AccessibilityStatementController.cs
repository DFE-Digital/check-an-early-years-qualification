using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController(
    ILogger<AccessibilityStatementController> logger,
    IContentService contentService,
    IHtmlRenderer renderer)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await contentService.GetAccessibilityStatementPage();

        if (content is null)
        {
            logger.LogError("No content for the accessibility statement page");
            return RedirectToAction("Error", "Home");
        }

        var model = await Map(content);

        return View(model);
    }

    private async Task<AccessibilityStatementPageModel> Map(AccessibilityStatementPage content)
    {
        return new AccessibilityStatementPageModel
               {
                   Heading = content.Heading,
                   BodyContent = await renderer.ToHtml(content.Body)
               };
    }
}
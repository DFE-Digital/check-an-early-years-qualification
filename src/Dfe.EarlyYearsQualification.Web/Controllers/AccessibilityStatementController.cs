using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController : Controller
{
    private readonly IContentService _contentService;
    private readonly ILogger<AccessibilityStatementController> _logger;
    private readonly IHtmlRenderer _renderer;

    public AccessibilityStatementController(
        ILogger<AccessibilityStatementController> logger,
        IContentService contentService,
        IHtmlRenderer renderer)
    {
        _logger = logger;
        _contentService = contentService;
        _renderer = renderer;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await _contentService.GetAccessibilityStatementPage();

        if (content is null)
        {
            _logger.LogError("No content for the accessibility statement page");
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
                   BodyContent = await _renderer.ToHtml(content.Body)
               };
    }
}
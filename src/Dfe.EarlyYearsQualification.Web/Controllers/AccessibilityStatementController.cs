using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Constants;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController : Controller
{
    private readonly ILogger<AccessibilityStatementController> _logger;
    private readonly IContentService _contentService;

    public AccessibilityStatementController(ILogger<AccessibilityStatementController> logger, IContentService contentService)
    {
        _logger = logger;
        _contentService = contentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await _contentService.GetAccessibilityStatementPage();

        if (content is null)
        {
            return RedirectToAction("Error", "Home");
        }

        var model = Map(content);

        return View(model);
    }

    private static AccessibilityStatementPageModel Map(AccessibilityStatementPage content)
    {
      return new AccessibilityStatementPageModel
      {
        Heading = content.Heading,
        BodyContent = content.BodyHtml
      };
    }
}

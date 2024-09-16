using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.RichTextParsing;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("accessibility-statement")]
public class AccessibilityStatementController(
    ILogger<AccessibilityStatementController> logger,
    IContentService contentService,
    IGovUkContentfulParser contentfulParser)
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

        var model = await Map(content);

        return View(model);
    }

    private async Task<AccessibilityStatementPageModel> Map(AccessibilityStatementPage content)
    {
        return new AccessibilityStatementPageModel
               {
                   Heading = content.Heading,
                   BodyContent = await contentfulParser.ToHtml(content.Body),
                   BackButton = MapToNavigationLinkModel(content.BackButton)
               };
    }
}
using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("cookies")]
public class CookiesController : Controller
{
  private readonly ILogger<CookiesController> _logger;
  private readonly IContentService _contentService;
  private readonly IHtmlTableRenderer _tableRenderer;
  private readonly ISuccessBannerRenderer _successBannerRenderer;

  public CookiesController(ILogger<CookiesController> logger, IContentService contentService, IHtmlTableRenderer tableRenderer, ISuccessBannerRenderer successBannerRenderer)
  {
    _logger = logger;
    _contentService = contentService;
    _tableRenderer = tableRenderer;
    _successBannerRenderer = successBannerRenderer;
  }

  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var content = await _contentService.GetCookiesPage();

    if (content is null)
    {
      _logger.LogError("No content for the cookies page");
      return RedirectToAction("Error", "Home");
    }

    var model = await Map(content);

    return View(model);
  }

  private async Task<CookiesPageModel> Map(CookiesPage content)
  {
    return new CookiesPageModel
    {
      Heading = content.Heading,
      BodyContent = await _tableRenderer.ToHtml(content.Body),
      Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
      ButtonText = content.ButtonText,
      SuccessBannerContent = await _successBannerRenderer.ToHtml(content.SuccessBannerContent),
      SuccessBannerHeading = content.SuccessBannerHeading,
      ErrorText = content.ErrorText
    };;
  }
}

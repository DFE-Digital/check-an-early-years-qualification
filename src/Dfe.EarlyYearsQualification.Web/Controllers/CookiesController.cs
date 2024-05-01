using Microsoft.AspNetCore.Mvc;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Content.Entities;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("cookies")]
public class CookiesController : Controller
{
  private readonly ILogger<CookiesController> _logger;
  private readonly IContentService _contentService;

  public CookiesController(ILogger<CookiesController> logger, IContentService contentService)
  {
    _logger = logger;
    _contentService = contentService;
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

    var model = Map(content);

    return View(model);
  }

  private static CookiesPageModel Map(CookiesPage content)
  {
    var test = new CookiesPageModel
    {
      Heading = content.Heading,
      BodyContent = content.BodyHtml,
      Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
      ButtonText = content.ButtonText,
      SuccessBannerContent = content.SuccessBannerContentHtml,
      SuccessBannerHeading = content.SuccessBannerHeading,
      ErrorText = content.ErrorText
    };

    return test;
  }
}

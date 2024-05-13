using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class PhaseBannerViewComponent : ViewComponent
{
  private readonly IContentService _contentService;
  private readonly ILogger<PhaseBannerViewComponent> _logger;

  public PhaseBannerViewComponent(IContentService contentService, ILogger<PhaseBannerViewComponent> logger)
  {
    _contentService = contentService;
    _logger = logger;
  }

  public async Task<IViewComponentResult> InvokeAsync()
  {
    var content = await _contentService.GetPhaseBannerContent();

    if (content is null)
    {
      _logger.LogError("No content for the phase banner");
      return View();
    }

    return View(content);
  }
}
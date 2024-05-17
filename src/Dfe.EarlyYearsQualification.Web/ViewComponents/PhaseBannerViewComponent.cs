using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class PhaseBannerViewComponent : ViewComponent
{
    private readonly IContentService _contentService;
    private readonly ILogger<PhaseBannerViewComponent> _logger;
    private readonly IPhaseBannerRenderer _renderer;

    public PhaseBannerViewComponent(
        IContentService contentService,
        ILogger<PhaseBannerViewComponent> logger,
        IPhaseBannerRenderer renderer)
    {
        _contentService = contentService;
        _logger = logger;
        _renderer = renderer;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var content = await _contentService.GetPhaseBannerContent();

        if (content is null)
        {
            _logger.LogError("No content for the phase banner");
            return View();
        }

        var model = await Map(content);

        return View(model);
    }

    private async Task<PhaseBannerModel> Map(PhaseBanner content)
    {
        return new PhaseBannerModel
               {
                   PhaseName = content.PhaseName,
                   Show = content.Show,
                   Content = await _renderer.ToHtml(content.Content)
               };
    }
}
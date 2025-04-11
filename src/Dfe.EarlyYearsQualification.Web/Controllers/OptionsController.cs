using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/options")]
public class OptionsController(
    ILogger<OptionsController> logger,
    ICachingOptionsManager cachingOptionsManager,
    IContentOptionsManager contentOptionsManager,
    IConfiguration config)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        logger.LogInformation("Options page accessed.");

        if (config["ENVIRONMENT"]?.StartsWith("prod", StringComparison.OrdinalIgnoreCase) == true)
        {
            return NotFound();
        }

        var model = new OptionsPageModel();

        await SetModelFromSelectedOptions(model);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(OptionsPageModel model)
    {
        logger.LogInformation("Options page submitted.");

        if (config["ENVIRONMENT"]?.StartsWith("prod", StringComparison.OrdinalIgnoreCase) == true)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await SetOptions(model.Option);
        }

        return RedirectToAction("Index", "Home");
    }

    private async Task SetModelFromSelectedOptions(OptionsPageModel model)
    {
        var option = await contentOptionsManager.GetContentOption();

        model.SetOption(option);
    }

    private async Task SetOptions(string option)
    {
        var cachingOption =
            option switch
            {
                OptionsPageModel.PublishedOptionValue => CachingOption.UseCache,
                OptionsPageModel.PreviewOptionValue => CachingOption.BypassCache,
                _ => CachingOption.UseCache
            };

        await cachingOptionsManager.SetCachingOption(cachingOption);

        var contentOption =
            option switch
            {
                OptionsPageModel.PublishedOptionValue => ContentOption.UsePublished,
                OptionsPageModel.PreviewOptionValue => ContentOption.UsePreview,
                _ => ContentOption.UsePublished
            };

        await contentOptionsManager.SetContentOption(contentOption);
    }
}
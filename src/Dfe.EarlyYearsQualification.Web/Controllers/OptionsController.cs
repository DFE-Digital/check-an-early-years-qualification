using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Services.Environments;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/options")]
public class OptionsController(
    ILogger<OptionsController> logger,
    ICachingOptionsManager cachingOptionsManager,
    IContentOptionsManager contentOptionsManager,
    IEnvironmentService environmentService)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        logger.LogInformation("Options page accessed.");

        if (environmentService.IsProduction())
        {
            return NotFound();
        }

        var model = new OptionsPageModel();

        await SetModelFromSelectedOption(model);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(OptionsPageModel model)
    {
        logger.LogInformation("Options page submitted.");

        if (environmentService.IsProduction())
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await SetOptions(model.Option);
        }

        return RedirectToAction("Index", "Home");
    }

    private async Task SetModelFromSelectedOption(OptionsPageModel model)
    {
        var option = await contentOptionsManager.GetContentOption();

        model.Option = option == ContentOption.UsePreview
                           ? OptionsPageModel.PreviewOptionValue
                           : OptionsPageModel.PublishedOptionValue;
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
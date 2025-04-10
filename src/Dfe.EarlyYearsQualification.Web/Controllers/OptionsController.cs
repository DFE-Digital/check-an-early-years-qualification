using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("/options")]
public class OptionsController(
    ILogger<OptionsController> logger,
    ICachingOptionsManager cachingOptionsManager,
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

        model.Option = model.Options.FirstOrDefault()!.Value;

        await UseCachingOptionForModel(model);

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
            await SetCachingOption(model.Option);
        }

        return RedirectToAction("Index", "Home");
    }

    private async Task UseCachingOptionForModel(OptionsPageModel model)
    {
        var option = await cachingOptionsManager.GetCachingOption();

        if (option != CachingOption.None)
        {
            model.Option = option.ToString();
        }
    }

    private async Task SetCachingOption(string option)
    {
        var optionOk = Enum.TryParse(option, out CachingOption cachingOption);

        if (optionOk)
        {
            await cachingOptionsManager.SetCachingOption(cachingOption);
        }
    }
}
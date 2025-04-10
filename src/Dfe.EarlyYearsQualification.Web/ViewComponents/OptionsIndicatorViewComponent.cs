using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class OptionsIndicatorViewComponent(
    ILogger<OptionsIndicatorViewComponent> logger,
    ICachingOptionsManager cachingOptionsManager,
    IConfiguration config) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        logger.LogInformation("Showing options view");

        var model = new OptionsPageModel();

        if (config["ENVIRONMENT"]?.StartsWith("prod", StringComparison.OrdinalIgnoreCase) == false)
        {
            var option = await cachingOptionsManager.GetCachingOption();

            model.Option = option.ToString();
        }

        return View(model);
    }
}
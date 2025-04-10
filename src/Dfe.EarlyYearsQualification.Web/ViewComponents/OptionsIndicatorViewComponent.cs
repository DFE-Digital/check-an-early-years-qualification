using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class OptionsIndicatorViewComponent(
    ILogger<OptionsIndicatorViewComponent> logger,
    ICachingOptionsManager cachingOptionsManager) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        logger.LogInformation("Showing options view");

        var option = await cachingOptionsManager.GetCachingOption();

        var model = new OptionsPageModel
                    {
                        Option = option.ToString()
                    };

        return View(model);
    }
}
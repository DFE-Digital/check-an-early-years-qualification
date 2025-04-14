using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class OptionsIndicatorViewComponent(
    ILogger<OptionsIndicatorViewComponent> logger,
    IContentOptionsManager contentOptionsManager,
    IConfiguration config) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        logger.LogInformation("Showing options view");

        var model = new OptionsPageModel();

        if (config["ENVIRONMENT"]?.StartsWith("prod", StringComparison.OrdinalIgnoreCase) == false)
        {
            var option = await contentOptionsManager.GetContentOption();

            model.Option = option == ContentOption.UsePreview
                               ? OptionsPageModel.PreviewOptionValue
                               : OptionsPageModel.PublishedOptionValue;
        }

        return View(model);
    }
}
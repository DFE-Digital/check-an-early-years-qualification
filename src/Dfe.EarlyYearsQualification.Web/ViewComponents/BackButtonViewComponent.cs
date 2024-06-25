using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.ViewComponents;

public class BackButtonViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(NavigationLink link)
    {
        return await Task.FromResult(View(link));
    }
}
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Renderers.Entities;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Web.Controllers.Base;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Dfe.EarlyYearsQualification.Web.Models.Content.QuestionModels;
using Dfe.EarlyYearsQualification.Web.Services.CookiesPreferenceService;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

[Route("cookies")]
public class CookiesPreferenceController(
    ILogger<CookiesPreferenceController> logger,
    IContentService contentService,
    IHtmlTableRenderer tableRenderer,
    ISuccessBannerRenderer successBannerRenderer,
    ICookiesPreferenceService cookieService,
    IUrlHelper urlHelper)
    : ServiceController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var content = await contentService.GetCookiesPage();

        if (content is null)
        {
            logger.LogError("No content for the cookies page");
            return RedirectToAction("Index", "Error");
        }

        var model = await Map(content);

        return View(model);
    }

    [HttpPost("accept")]
#pragma warning disable S6967
    // ...the model is a string, so no need to check ModelState.IsValid here
    public IActionResult Accept([FromForm] string? returnUrl)
#pragma warning restore S6967
    {
        cookieService.SetPreference(true);
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost("reject")]
#pragma warning disable S6967
    // ...the model is a string, so no need to check ModelState.IsValid here
    public IActionResult Reject([FromForm] string? returnUrl)
#pragma warning restore S6967
    {
        cookieService.RejectCookies();
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost("hidebanner")]
#pragma warning disable S6967
    // ...the model is a string, so no need to check ModelState.IsValid here
    public IActionResult HideBanner([FromForm] string? returnUrl)
#pragma warning restore S6967
    {
        cookieService.SetVisibility(false);
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost]
#pragma warning disable S6967
    // ...the model is a string, so no need to check ModelState.IsValid here
    public IActionResult CookiePreference(string cookiesAnswer)
#pragma warning restore S6967
    {
        if (cookiesAnswer == "all-cookies")
        {
            cookieService.SetPreference(true);
        }
        else
        {
            cookieService.RejectCookies();
        }

        TempData["UserPreferenceRecorded"] = true;
        return Redirect("/cookies");
    }

    private string CheckUrl(string? url)
    {
        return urlHelper.IsLocalUrl(url) ? url : "/cookies";
    }

    private async Task<CookiesPageModel> Map(CookiesPage content)
    {
        return new CookiesPageModel
               {
                   Heading = content.Heading,
                   BodyContent = await tableRenderer.ToHtml(content.Body),
                   Options = content.Options.Select(x => new OptionModel { Label = x.Label, Value = x.Value }).ToList(),
                   ButtonText = content.ButtonText,
                   SuccessBannerContent = await successBannerRenderer.ToHtml(content.SuccessBannerContent),
                   SuccessBannerHeading = content.SuccessBannerHeading,
                   ErrorText = content.ErrorText,
                   BackButton = MapToNavigationLinkModel(content.BackButton),
                   FormHeading = content.FormHeading
               };
    }
}
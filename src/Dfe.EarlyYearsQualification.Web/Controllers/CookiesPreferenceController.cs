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
    public IActionResult Accept([FromForm] string? returnUrl)
    {
        cookieService.SetPreference(true);
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost("reject")]
    public IActionResult Reject([FromForm] string? returnUrl)
    {
        cookieService.RejectCookies();
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost("hidebanner")]
    public IActionResult HideBanner([FromForm] string? returnUrl)
    {
        cookieService.SetVisibility(false);
        return Redirect(CheckUrl(returnUrl));
    }

    [HttpPost]
    public IActionResult CookiePreference(string cookiesAnswer)
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
                   BackButton = content.BackButton,
                   FormHeading = content.FormHeading
               };
    }
}
using Dfe.EarlyYearsQualification.Content.Entities;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Dfe.EarlyYearsQualification.Web.Filters;
using Dfe.EarlyYearsQualification.Web.Helpers;
using Dfe.EarlyYearsQualification.Web.Mappers.Interfaces;
using Dfe.EarlyYearsQualification.Web.Models;
using Dfe.EarlyYearsQualification.Web.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.EarlyYearsQualification.Web.Controllers;

public class ChallengeController(
    ILogger<ChallengeController> logger,
    IUrlHelper urlHelper,
    IContentService contentService,
    ICheckServiceAccessKeysHelper accessKeysHelper,
    IChallengePageMapper challengePageMapper)
    : Controller
{
    private const string DefaultRedirectAddress = "/";

    [HttpGet("/challenge")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Index([FromQuery] ChallengePageModel model)
    {
        if (accessKeysHelper.AllowPublicAccess)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid challenge model (get)");
        }

        logger.LogWarning("Challenge page invoked");

        var contentModel = await contentService.GetChallengePage();

        if (contentModel == null)
        {
            logger.LogError("No content for the challenge page");
            return RedirectToAction("Index", "Error");
        }

        var pageModel = await Map(model, contentModel);

        return View("EntryForm", pageModel);
    }

    [HttpPost("/challenge")]
    public async Task<IActionResult> Post([FromForm] ChallengePageModel model)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid challenge model (post)");
        }

        var referralAddress = SanitiseReferralAddress(model.RedirectAddress);

        var contentModel = await contentService.GetChallengePage();

        if (contentModel == null)
        {
            logger.LogError("No content for the challenge page");
            return RedirectToAction("Index", "Error");
        }

        if (string.IsNullOrWhiteSpace(model.PasswordValue))
        {
            return await ReturnWithError(contentModel.MissingPasswordText, model, contentModel);
        }

        model.PasswordValue = model.PasswordValue.Trim();

        logger.LogInformation("Challenge secret access value entered");

        if (!accessKeysHelper.ConfiguredKeys.Contains(model.PasswordValue))
        {
            return await ReturnWithError(contentModel.IncorrectPasswordText, model, contentModel);
        }

        SetAuthSecretCookie(model.PasswordValue);
        return new RedirectResult(referralAddress);
    }

    private async Task<IActionResult> ReturnWithError(string errorMessage, ChallengePageModel model,
                                                      ChallengePage contentModel)
    {
        model.ErrorSummaryModel = new ErrorSummaryModel
                                  {
                                      ErrorBannerHeading = contentModel.ErrorHeading,
                                      ErrorSummaryLinks =
                                      [
                                          new ErrorSummaryLink
                                          {
                                              ErrorBannerLinkText = errorMessage,
                                              ElementLinkId = "PasswordValue"
                                          }
                                      ]
                                  };

        model = await Map(model, contentModel);

        return View("EntryForm", model);
    }

    private void SetAuthSecretCookie(string accessValue)
    {
        HttpContext.Response
                   .Cookies
                   .Append(ChallengeResourceFilterAttribute.AuthSecretCookieName,
                           accessValue,
                           new CookieOptions { HttpOnly = true, Secure = true });
    }

    private string SanitiseReferralAddress(string from)
    {
        var redirectAddress = from;

        if (!urlHelper.IsLocalUrl(redirectAddress))
        {
            redirectAddress = DefaultRedirectAddress;
        }

        return redirectAddress;
    }

    private async Task<ChallengePageModel> Map(ChallengePageModel model, ChallengePage content)
    {
        var sanitisedReferralAddress = SanitiseReferralAddress(model.RedirectAddress);
        return await challengePageMapper.Map(model, content, sanitisedReferralAddress);
    }
}
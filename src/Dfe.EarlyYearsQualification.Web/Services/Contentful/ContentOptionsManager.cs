using Dfe.EarlyYearsQualification.Content.Options;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public class ContentOptionsManager(
    ILogger<ContentOptionsManager> logger,
    ICookieManager cookieManager) : IContentOptionsManager
{
    public const string OptionsCookieKey = "contentOption";

    public Task<ContentOption> GetContentOption()
    {
        logger.LogInformation("Getting user's content option");

        var cookies = cookieManager.ReadInboundCookies();

        if (cookies == null
            || !cookies.TryGetValue(OptionsCookieKey, out string? optionVal)
            || string.IsNullOrWhiteSpace(optionVal))
        {
            logger.LogInformation("User's content option not set, using default value");
            return Task.FromResult(ContentOption.UsePublished);
        }

        bool ok = Enum.TryParse(optionVal, out ContentOption optionEnum);

        if (ok)
        {
            return Task.FromResult(optionEnum);
        }

        logger.LogWarning("User's content option set to unexpected value '{OptionVal}'", optionVal);

        return Task.FromResult(ContentOption.UsePublished);
    }

    public Task SetContentOption(ContentOption option)
    {
        logger.LogInformation("Setting user's content option to {Option}", option);

        var cookieOptions = cookieManager.CreateCookieOptions(DateTimeOffset.UtcNow.AddDays(1), true);
        cookieManager.SetOutboundCookie(OptionsCookieKey, option.ToString(), cookieOptions);

        return Task.CompletedTask;
    }
}
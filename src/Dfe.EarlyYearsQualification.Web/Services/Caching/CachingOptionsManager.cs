using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public class CachingOptionsManager(
    ILogger<CachingOptionsManager> logger,
    ICookieManager cookieManager) : ICachingOptionsManager
{
    public const string OptionsCookieKey = "cachingOption";

    public Task<CachingOption> GetCachingOption()
    {
        logger.LogInformation("Getting user's caching option");

        var cookies = cookieManager.ReadInboundCookies();

        if (cookies == null
            || !cookies.TryGetValue(OptionsCookieKey, out string? optionVal)
            || string.IsNullOrWhiteSpace(optionVal))
        {
            logger.LogInformation("User's caching option not set, using default value");
            return Task.FromResult(CachingOption.UseCache);
        }

        bool ok = Enum.TryParse(optionVal, out CachingOption optionEnum);

        if (ok)
        {
            return Task.FromResult(optionEnum);
        }

        logger.LogWarning("User's caching option set to unexpected value '{OptionVal}'", optionVal);

        return Task.FromResult(CachingOption.UseCache);
    }

    public Task SetCachingOption(CachingOption option)
    {
        logger.LogInformation("Setting user's caching option to {Option}", option);

        var cookieOptions = cookieManager.CreateCookieOptions(DateTimeOffset.UtcNow.AddDays(1), true);
        cookieManager.SetOutboundCookie(OptionsCookieKey, option.ToString(), cookieOptions);

        return Task.CompletedTask;
    }
}
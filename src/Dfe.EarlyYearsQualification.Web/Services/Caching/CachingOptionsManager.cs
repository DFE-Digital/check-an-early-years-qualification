using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Web.Services.Cookies;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

public class CachingOptionsManager(
    ILogger<CachingOptionsManager> logger,
    ICookieManager cookieManager) : ICachingOptionsManager
{
    private const string OptionsCookieKey = "option";

    public Task<CachingOption> GetCachingOption()
    {
        logger.LogInformation("Getting user's caching option");

        var cookies = cookieManager.ReadInboundCookies();

        if (cookies == null
            || !cookies.TryGetValue(OptionsCookieKey, out var optionVal)
            || string.IsNullOrWhiteSpace(optionVal))
        {
            logger.LogInformation("User's caching option not set, using default value");
            return Task.FromResult(CachingOption.None);
        }

        var ok = Enum.TryParse<CachingOption>(optionVal, out var optionEnum);

        if (ok)
        {
            return Task.FromResult(optionEnum);
        }

        logger.LogWarning("User's caching option set to unexpected value {OptionVal}", optionVal);

        return Task.FromResult(CachingOption.None);
    }

    public Task SetCachingOption(CachingOption option)
    {
        logger.LogInformation("Setting user's caching option to {Option}", option);

        var cookieOptions = new CookieOptions
                            {
                                Expires = DateTimeOffset.UtcNow.AddDays(1),
                                HttpOnly = true,
                                Secure = true
                            };

        cookieManager.SetOutboundCookie(OptionsCookieKey, option.ToString(), cookieOptions);

        return Task.CompletedTask;
    }
}
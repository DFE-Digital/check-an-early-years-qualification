using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Caching;

/// <summary>
///     An <see cref="ICachingOptionsManager" />, that always specifies caching behaviour.
/// </summary>
public class NeverBypassCacheManager : ICachingOptionsManager
{
    public Task<CachingOption> GetCachingOption()
    {
        return Task.FromResult(CachingOption.UseCache);
    }

    public Task SetCachingOption(CachingOption option)
    {
        return Task.CompletedTask;
    }
}
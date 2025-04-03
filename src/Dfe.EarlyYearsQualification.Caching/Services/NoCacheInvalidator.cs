using Dfe.EarlyYearsQualification.Caching.Interfaces;

namespace Dfe.EarlyYearsQualification.Caching.Services;

public class NoCacheInvalidator : ICacheInvalidator
{
    public Task ClearCacheAsync(string keyPrefix)
    {
        return Task.CompletedTask;
    }
}
namespace Dfe.EarlyYearsQualification.Caching.Interfaces;

public interface ICachingOptionsManager
{
    public Task<CachingOption> GetCachingOption();

    public Task SetCachingOption(CachingOption option);
}
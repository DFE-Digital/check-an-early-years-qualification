namespace Dfe.EarlyYearsQualification.Caching.Interfaces;

public interface ICacheInvalidator
{
    Task ClearCacheAsync();
}
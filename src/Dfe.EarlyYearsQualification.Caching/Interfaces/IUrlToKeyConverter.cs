namespace Dfe.EarlyYearsQualification.Caching.Interfaces;

public interface IUrlToKeyConverter
{
    Task<string> GetKeyAsync(Uri uri);
}
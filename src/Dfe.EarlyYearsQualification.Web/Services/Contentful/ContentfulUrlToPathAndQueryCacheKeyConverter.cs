using Dfe.EarlyYearsQualification.Caching.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public class ContentfulUrlToPathAndQueryCacheKeyConverter : IUrlToKeyConverter
{
    /// <summary>
    ///     Cache key prefix for Check An Early Years Qualification service: Contentful
    /// </summary>
    public const string KeyPrefix = "ceyq/contentful:";

    public Task<string> GetKeyAsync(Uri uri)
    {
        var pathAndQuery = uri.PathAndQuery;

        return Task.FromResult($"{KeyPrefix}{pathAndQuery}");
    }
}
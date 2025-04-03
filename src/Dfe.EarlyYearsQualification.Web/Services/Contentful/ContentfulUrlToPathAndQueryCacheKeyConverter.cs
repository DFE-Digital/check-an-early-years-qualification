using Dfe.EarlyYearsQualification.Caching.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public class ContentfulUrlToPathAndQueryCacheKeyConverter : IUrlToKeyConverter
{
    public const string KeyPrefix = "contentful:";

    public Task<string> GetKeyAsync(Uri uri)
    {
        var pathAndQuery = uri.PathAndQuery;

        return Task.FromResult($"{KeyPrefix}{pathAndQuery}");
    }
}
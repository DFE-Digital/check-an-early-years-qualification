using Dfe.EarlyYearsQualification.Caching.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public class ContentfulUrlToCacheKeyConverter : IUrlToKeyConverter
{
    public Task<string> GetKeyAsync(Uri uri)
    {
        var pathAndQuery = uri.PathAndQuery;

        return Task.FromResult($"contentful:{pathAndQuery}");
    }
}
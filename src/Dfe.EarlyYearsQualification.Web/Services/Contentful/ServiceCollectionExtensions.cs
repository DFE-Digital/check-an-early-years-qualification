using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;

namespace Dfe.EarlyYearsQualification.Web.Services.Contentful;

public static class ServiceCollectionExtensions
{
    // "ContentfulClient" is set in Contentful.AspNetCore.IServiceCollectionExtensions.HttpClientName
    private const string ContentfulClientHttpClientName = "ContentfulClient";

    /// <summary>
    ///     Sets up the Contentful services for <see cref="IContentService" /> and <see cref="IQualificationsRepository" />.
    ///     Also sets up the HttpMessageHandler for the HttpClient used by Contentful to cache use the distributed
    ///     cache for all responses received from Contentful.
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void SetupContentfulServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IContentService, ContentfulContentService>();
        serviceCollection.AddTransient<IQualificationsRepository, QualificationsRepository>();

        serviceCollection.AddHttpClient(ContentfulClientHttpClientName)
                         .ConfigurePrimaryHttpMessageHandler<CachingHandler>();
    }
}
using System.Net;
using Contentful.Core.Configuration;
using Dfe.EarlyYearsQualification.Caching;
using Dfe.EarlyYearsQualification.Caching.Interfaces;
using Dfe.EarlyYearsQualification.Content.Services;
using Dfe.EarlyYearsQualification.Content.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

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
        serviceCollection.AddScoped<IContentService, ContentfulContentService>();
        serviceCollection.AddScoped<IQualificationsRepository, QualificationsRepository>();

        serviceCollection.AddScoped<HttpClientHandler>(sp => new HttpClientHandler());

        serviceCollection.AddHttpClient(ContentfulClientHttpClientName)
                         .ConfigurePrimaryHttpMessageHandler(ConfigureHandler);
    }

    private static HttpMessageHandler ConfigureHandler(IServiceProvider serviceProvider)
    {
        var options = serviceProvider.GetService<IOptions<ContentfulOptions>>()!.Value;

        var httpHandler = serviceProvider.GetService<HttpClientHandler>()!;

        if (options.AllowHttpResponseCompression && httpHandler.SupportsAutomaticDecompression)
        {
            httpHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        }

        var cache = serviceProvider.GetService<IDistributedCache>()!;
        var urlToKeyConverter = serviceProvider.GetService<IUrlToKeyConverter>()!;
        var cachingOptionsManager = serviceProvider.GetService<ICachingOptionsManager>()!;
        var logger = serviceProvider.GetService<ILogger<CachingHandler>>()!;

        return new CachingHandler(cache,
                                  urlToKeyConverter,
                                  cachingOptionsManager,
                                  logger,
                                  httpHandler);
    }
}